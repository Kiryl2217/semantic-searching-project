using Microsoft.EntityFrameworkCore;
using SemanticSearch.Models;

namespace SemanticSearch.Repositories
{
    public class DocumentsRepository : IRepository<Documents>
    {
        private ApplicationContext DB;
        public DocumentsRepository(ApplicationContext DB)
        {
            this.DB = DB;
        }

        public void Create(Documents item)
        {
            DB.Documents.Add(item);
            DB.SaveChanges();
        }

        public Documents Read(int id)
            => DB.Documents.Find(id);
        
        public DbSet<Documents> Read()
            => DB.Documents;
        
        public void Update(int id, Documents item)
        {
            DB.Documents.Update(item);
            DB.SaveChanges();
        }

        public void Delete(int id)
        {
            var document = DB.Documents.Find(id);
            DB.Documents.Remove(document);
            DB.SaveChanges();
        }
    }
}
