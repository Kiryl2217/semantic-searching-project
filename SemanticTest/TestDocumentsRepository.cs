using SemanticSearch.Models;
using SemanticSearch.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticTest
{
    public class TestDocumentsRepository
    {
        //private string testConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\visual\\" +
        //    "SemanticSearch\\SemanticSearch\\bin\\Debug\\net8.0\\" +
        //    "SemanticSearchTestDB.mdf;Integrated Security=True;Connect Timeout=30";

        [Fact]
        public void TestCreateDocumentsRepository()
        {
            var DB = new ApplicationContext(testConnectionString);
            var documentsRepository = new DocumentsRepository(DB);

            documentsRepository.Create(new Documents
            {
                Id = 1,
                DateAdded = DateTime.Now,
                Title = "Тест",
                OriginalText = "Тест"
            });

            Assert.Equal(1, DB.Documents.Count());
            Assert.Equal("Тест", DB.Documents.First().Title);
        }

        [Fact]
        public void TestReadByIdDocumentsRepository()
        {
            var DB = new ApplicationContext(testConnectionString);
            var documentsRepository = new DocumentsRepository(DB);

            var result = documentsRepository.Read(1);

            Assert.NotNull(result);
            Assert.Equal("Тест", result.Title);
        }

        [Fact]
        public void TestReadDocumentsRepository()
        {
            var DB = new ApplicationContext(testConnectionString);
            var documentsRepository = new DocumentsRepository(DB);

            documentsRepository.Create(new Documents
            {
                Id = 2,
                DateAdded = DateTime.Now,
                Title = "Тест2",
                OriginalText = "Тест2"
            });

            var result = documentsRepository.Read();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void TestUpdateDocumentsRepository()
        {
            var DB = new ApplicationContext(testConnectionString);
            var documentsRepository = new DocumentsRepository(DB);

            var newDoc = new Documents
            {
                Id = 1,
                DateAdded = DateTime.Now,
                Title = "Тест01",
                OriginalText = "Тест01"
            };

            documentsRepository.Update(1, newDoc);

            Assert.Equal("Тест01", DB.Documents.First().Title);
        }

        [Fact]
        public void TestDeleteDocumentsRepository()
        {
            var DB = new ApplicationContext(testConnectionString);
            var documentsRepository = new DocumentsRepository(DB);

            documentsRepository.Delete(1);
            documentsRepository.Delete(2);

            Assert.Equal(0, DB.Documents.Count());
        }
    }
}
