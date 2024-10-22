using DoiFApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DoiFApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<LessonModel> Lessons { get; set; }
        public DbSet<EducationTeacherModel> EducationTeachers { get; set; }
        public DbSet<EducationWorkModel> EducationWorks{ get; set; }
        public DbSet<EducationTypeAndHourModel> EducationTypesAndHours { get; set; }
        public DbSet<NonEducationWork> NonEducationWorks {  get; set; }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        public void RecreateLessons()
        {
            foreach (var entity in ChangeTracker.Entries())
                entity.State = EntityState.Detached;

            try
            {
                Lessons.RemoveRange(Lessons);
            }
            catch
            {
            }
            SaveChanges();
        }

        public void RecreateEducation()
        {
            foreach (var entity in ChangeTracker.Entries())
                entity.State = EntityState.Detached;

            try
            {
                EducationTeachers.RemoveRange(EducationTeachers);
                EducationWorks.RemoveRange(EducationWorks);
                EducationTypesAndHours.RemoveRange(EducationTypesAndHours);
            }
            catch
            {
            }
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=doifapp.db");
        }
    }
}
