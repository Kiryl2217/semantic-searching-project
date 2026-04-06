using Microsoft.EntityFrameworkCore;
using SemanticSearch.Models;
using SemanticSearch.Repositories;
using System.Reflection.Metadata;

namespace SemanticSearch.Services
{
    public class SequentialWordsService : IService<SequentialWords>
    {
        private readonly SequentialWordsRepository sequentialWordsRepository;

        public SequentialWordsService(ApplicationContext DB)
        {
            sequentialWordsRepository = new SequentialWordsRepository(DB);
        }

        public void Add(SequentialWords item)
        {
            sequentialWordsRepository.Create(item);
        }

        public SequentialWords Get(int id)
            => sequentialWordsRepository.Read(id);
        
        public List<SequentialWords> Get()
            => sequentialWordsRepository.Read().ToList();

        public void Update(int id, SequentialWords item)
        {
            sequentialWordsRepository.Update(id, item);
        }

        public void Delete(int id)
        {
            sequentialWordsRepository.Delete(id);
        }

        public string[] GetUniqueWordsByDocumentsId(int documentsId)
        => sequentialWordsRepository.Read()
                .Where(sw => sw.DocumentsId == documentsId)
                .Select(sw => sw.Word).Distinct().ToArray();

        public void WriteWordsDB(int doumentsId, IEnumerable<string> words)
            => words.Select(w => new SequentialWords()
            {
                DocumentsId = doumentsId,
                Word = w
            })
            .ToList()
            .ForEach(sequentialWordsRepository.Create);
    }
}
