using DoIFToolApp.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DoiFApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<LessonModel> Lessons { get; set; }

        public void ClearDb()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=doifapp.db");
        }
    }
}
