using Microsoft.EntityFrameworkCore;

namespace EFCoreWebDemo
{
    public class EFCoreWebDemoContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        
        public string databasePath { get; }

        public EFCoreWebDemoContext()
        {
            string path = Environment.CurrentDirectory;
            databasePath = Path.Join(path, "getting-started.db");
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={databasePath}");
    }
}