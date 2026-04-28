using Microsoft.AspNetCore.Mvc;
using SemanticSearch.Classes;
using SemanticSearch.Models;
using SemanticSearch.Services;
using SemanticSearch.Structures;
using System.Diagnostics;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.Json.Nodes;


namespace SemanticSearch.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment appEnvironment;
        private readonly DocumentsService documentsService;

        

        public HomeController(IConfiguration config, IWebHostEnvironment appEnvironment)
        {
            this.config = config;
            this.appEnvironment = appEnvironment;
            var DB = new ApplicationContext(config.GetConnectionString("ConnectionString"));
            documentsService = new DocumentsService(DB);
        }


        public IActionResult Index()
        {
            ViewBag.NewAdd = true;
            return View();
        }


        [HttpPost]
        public IActionResult Index(IFormFile inputFile)
        {
            ViewBag.NewAdd = true;



            if (inputFile == null || inputFile.Length == 0)
            {
                ViewBag.ErrorFile = "Ошибка! Вы не выбрали файл или он пустой!";
                return View();
            }



            if (!(inputFile.FileName.EndsWith(".txt")
                || inputFile.FileName.EndsWith(".docx")
                || inputFile.FileName.EndsWith(".pdf")))
            {
                ViewBag.ErrorFile = "Ошибка! Вы выбрали не .txt (.docx, .pdf) файл!";
                return View();
            }

            // Чтение текста документа
            string documentText = "";







            if (inputFile.FileName.EndsWith(".txt"))
            {
                using (var reader = new StreamReader(inputFile.OpenReadStream()))
                {
                    documentText = reader.ReadToEnd();
                }
            } 
            else if (inputFile.FileName.EndsWith(".docx"))
            {
                using (var doc = WordprocessingDocument.Open(inputFile.OpenReadStream(), false)) 
                {

                    var paragraphs = doc.MainDocumentPart.Document.Body.Descendants<Paragraph>();

                    documentText = string.Empty;
                    paragraphs.ToList().ForEach(paragraph => documentText += paragraph.InnerText.ToString() + "\n");
                }
            }
            else
            {
                using (var reader = inputFile.OpenReadStream())
                {
                    using (var pdfDocument = UglyToad.PdfPig.PdfDocument.Open(reader))
                    {
                        var paragraphs = new List<string>();

                        foreach (var page in pdfDocument.GetPages())
                        {
                            var words = page.GetWords();
                            var blocks = UglyToad.PdfPig.DocumentLayoutAnalysis
                                .PageSegmenter.DocstrumBoundingBoxes.Instance.GetBlocks(words);

                            foreach (var block in blocks)
                            {
                                paragraphs.Add(block.Text.Trim());
                            }
                        }

                        documentText = string.Join("\n", paragraphs);
                    }
                }
            }





            if (string.IsNullOrWhiteSpace(documentText))
            {
                ViewBag.ErrorDoc = "Ошибка! В файле нет текста!";
                return View();
            }
           
            var documentFromDB = documentsService.GetByText(documentText);

            if (documentFromDB != null)
            {
                ViewBag.ErrorDoc = "Ошибка! Такой текст уже есть в базе данных!";
            }
            else
            {
                // Работаем с синонимами
                var textFile = Constants.SYNONYMS.ToLower().Replace("ё", "е");
                
                // Разбор строк с синонимами
                var stringsFromFile = textFile.Split("\n");
                
                stringsFromFile = stringsFromFile.Where(t => t.Contains("см.")).ToArray(); // Все с см.
                var arrWordAndSyn = stringsFromFile.Select(s =>
                    s.Split("см.").Select(s => s.Trim()).ToArray()) // Два элемнта массива: слово(строка) и набор синонимом (строка)
                    .ToArray();

                // Разбор текста + заносим в базу данных разбор слов
                Analyzer analyzer = new Analyzer(config);

                var dicRootSyn = new Dictionary<string, List<string>>();
                foreach (var wordAndSyn in arrWordAndSyn)
                {
                    var word000 = wordAndSyn[0]; // слово
                    analyzer.Division(word000, out _, out List<Root> roots, out _, out _);


                    var syns = wordAndSyn[1]
                        .Split(",")
                        .Select(s => s.Trim())
                        .Select(syn =>
                        {
                            analyzer.Division(syn, out _, out List<Root> synRoots, out _, out _);
                            return synRoots;
                        })
                        .SelectMany(s => s)
                        .Select(s=>s.root)
                        .Distinct()
                        .ToList();

                    // roots может содержать одинаковые корни различных частей речи
                    roots.Select(s => s.root).Distinct()
                        .Where(s => !dicRootSyn.ContainsKey(s))
                        .ToList()
                        .ForEach(root => dicRootSyn.Add(root, syns));
                }
               








                var words = analyzer.GetManyWithDB(documentText);
                analyzer.Division(words, dicRootSyn); // Здесь результат-словарь игнорируем, добавляем синонимы

                ViewBag.message = "Успешно добавлено";
                ViewBag.NewAdd = false;
            }
            
            return View();
        }
       


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
