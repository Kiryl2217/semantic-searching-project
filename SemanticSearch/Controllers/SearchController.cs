using Microsoft.AspNetCore.Mvc;
using SemanticSearch.Classes;

namespace SemanticSearch.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IConfiguration config;
        public SearchController(IConfiguration config)
        {
            this.config = config;
        }
        public IActionResult Index()
        {
            return View(null);
        }

        [HttpPost]
        public IActionResult Index(string userRequest)
        {
            ViewBag.UserRequest = userRequest;

            Analyzer analyzer = new Analyzer(config);

            /*
            // Получаем текст из файла
            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory +"/doc.txt");
            var doc = reader.ReadToEnd();
            */

            var documents = analyzer.GetAllDocuments(); // получим все записи документов через сервис

            var globalMaxRating = 0;
            var globalString = string.Empty;
            foreach (var document in documents)
            {

                var originalText = document.OriginalText;

                // Получаем из текста все !!!уникальные!!!!! форматированные слова
                // или из базы данных уже готовые форматированные ,
                var words = analyzer.GetManyWithDB(originalText);

                // получаем словарь, где каждому слову из текста
                // сопоставлен его вектор
                var dictionaryWithUniqueWords = analyzer.Division(words);




                // разбиваем текст на абзацы
                var originalParagraphs = originalText.Split('\n').ToList(); // ЭТО ОРИГИНАЛ.

                // сравниваем userRequest с каждым абзацем В ОРИГИНАЛЕ!!!!! 
                // но передаём словарь dic с уникальными уже разобранными словами.
                var paragraphRatings = originalParagraphs.Select(paragraph => 
                    analyzer.GetCompareParagraph(userRequest, paragraph, dictionaryWithUniqueWords)).ToArray();


                // находим номер релевантного абзаца
                // indexMaxRating - индекс
                // globalMaxRating - число рейтинга
                // globalString - релевантный абзац
                var indexMaxRating = 0;
                for (var i = 1; i < paragraphRatings.Count(); i++)
                    if (paragraphRatings[indexMaxRating] < paragraphRatings[i])
                        indexMaxRating = i;

                if (paragraphRatings[indexMaxRating] > globalMaxRating)
                {
                    globalMaxRating = paragraphRatings[indexMaxRating];
                    globalString = originalParagraphs[indexMaxRating];
                }

            }

            if (globalMaxRating == 0)
                globalString = "не найдено";

            return View((object?)globalString);
        }
    }
}
