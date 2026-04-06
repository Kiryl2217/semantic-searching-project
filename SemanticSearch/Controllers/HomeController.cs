using Microsoft.AspNetCore.Mvc;
using SemanticSearch.Classes;
using SemanticSearch.Models;
using SemanticSearch.Services;
using System.Diagnostics;
using System.IO;
using System.Web;

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

            // путь к папке files


            if (inputFile == null || inputFile.Length == 0)
            {
                ViewBag.ErrorFile = "Ошибка! Вы не выбрали файл или он пустой!";
                return View();
            }

            string path = "/files/" + inputFile.FileName;


            if (!inputFile.FileName.EndsWith(".txt"))
            {
                ViewBag.ErrorFile = "Ошибка! Вы выбрали не .txt файл!";
                return View();
            }

            // Чтение текста документа.
            string documentText = "";
            using (var reader = new StreamReader(inputFile.OpenReadStream()))
            {
                documentText = reader.ReadToEnd();
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
                /*
                // записываем ОРИГИНАЛЬНЫЙ текст в документ
                documentsService.Add(new Documents()
                {
                    OriginalText = documentText,
                    DateAdded = DateTime.Now,
                    Title = "Документ на " + DateTime.Now.ToString("dd.MM.yyyy")
                });
                */

                // Разбор текста + заносим в базу данных разбор слов.
                Analyzer analyzer = new Analyzer(config);
                var words = analyzer.GetManyWithDB(documentText);
                analyzer.Division(words); // Здесь результат-словарь игнорируем.

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
