using SemanticSearch.Models;

namespace SemanticTest
{
    public class TestModels
    {
        [Fact]
        public void TestDocuments()
        {
            var entity = new Documents
            {
                Id = 1,
                DateAdded = DateTime.Now,
                Title = "Тест",
                OriginalText = "Тест"
            };

            Assert.NotNull(entity);
            if (entity != null)
            {
                Assert.Equal(1, entity.Id);
                Assert.Equal("Тест", entity.Title);
            }
        }

        [Fact]
        public void TestSequentialWords()
        {
            var entity = new SequentialWords
            {
                Id = 1,
                Word = "тестовый",
                DocumentsId = 1
            };

            Assert.NotNull(entity);
            if (entity != null)
            {
                Assert.Equal(1, entity.Id);
                Assert.Equal("тестовый", entity.Word);
                Assert.Equal(1, entity.DocumentsId);
            }
        }

        [Fact]
        public void TestUniqueWords()
        {
            var entity = new UniqueWords
            {
                Id = 1,
                OriginalWord = "Тестовый"
            };

            Assert.NotNull(entity);
            if (entity != null)
            {
                Assert.Equal(1, entity.Id);
                Assert.Equal("Тестовый", entity.OriginalWord);
            }
        }

        [Fact]
        public void TestEndings()
        {
            var entity = new Endings
            {
                Id = 1,
                Ending = "ый",
                GenderId = 1,
                PluralityId = 1,
                DeclensionId = 1,
                PartOfSpeechId = 1,
                UniqueWordsId = 1
            };

            Assert.NotNull(entity);
            if (entity != null)
            {
                Assert.Equal(1, entity.Id);
                Assert.Equal("ый", entity.Ending);
                Assert.Equal(1, entity.GenderId);
                Assert.Equal(1, entity.PluralityId);
                Assert.Equal(1, entity.DeclensionId);
                Assert.Equal(1, entity.PartOfSpeechId);
                Assert.Equal(1, entity.UniqueWordsId);
            }
        }

        [Fact]
        public void TestPrefixes()
        {
            var entity = new Prefixes
            {
                Id = 1,
                Prefix = "т",
                PartOfSpeechId = 1,
                UniqueWordsId = 1
            };

            Assert.NotNull(entity);
            if (entity != null)
            {
                Assert.Equal(1, entity.Id);
                Assert.Equal("т", entity.Prefix);
                Assert.Equal(1, entity.PartOfSpeechId);
                Assert.Equal(1, entity.UniqueWordsId);
            }
        }

        [Fact]
        public void TestRoots()
        {
            var entity = new Roots
            {
                Id = 1,
                Root = "тест",
                PartOfSpeechId = 1,
                UniqueWordsId = 1
            };

            Assert.NotNull(entity);
            if (entity != null)
            {
                Assert.Equal(1, entity.Id);
                Assert.Equal("тест", entity.Root);
                Assert.Equal(1, entity.PartOfSpeechId);
                Assert.Equal(1, entity.UniqueWordsId);
            }
        }

        [Fact]
        public void TestSuffixes()
        {
            var entity = new Suffixes
            {
                Id = 1,
                Suffix = "ов",
                SuffixTypeId = 1,
                SuffixWithEndingId = 1,
                PartOfSpeechId = 1,
                UniqueWordsId = 1
            };

            Assert.NotNull(entity);
            if (entity != null)
            {
                Assert.Equal(1, entity.Id);
                Assert.Equal("ов", entity.Suffix);
                Assert.Equal(1, entity.SuffixTypeId);
                Assert.Equal(1, entity.SuffixWithEndingId);
                Assert.Equal(1, entity.PartOfSpeechId);
                Assert.Equal(1, entity.UniqueWordsId);
            }
        }
    }
}