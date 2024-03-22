using DoiFApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DoiFApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<LessonModel> Lessons { get; set; }

        public void Recreate()
        {
            foreach (var entity in ChangeTracker.Entries())
                entity.State = EntityState.Detached;

            Database.EnsureDeleted();
            Database.EnsureCreated();
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=doifapp.db");
        }
    }
}
