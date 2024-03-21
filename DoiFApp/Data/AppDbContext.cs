using DoiFApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DoiFApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<LessonModel> Lessons { get; set; }

        public void Recreate()
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
