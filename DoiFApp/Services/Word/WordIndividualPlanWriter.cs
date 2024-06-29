using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Utils;
using System.IO;
using System.Windows.Forms;
using Xceed.Words.NET;

namespace DoiFApp.Services.Word
{
    public class WordIndividualPlanWriter(IRepo<LessonModel> lessonRepo) : IIndividualPlanWriter
    {
        private readonly IRepo<LessonModel> lessonRepo = lessonRepo;

        public async Task MakePlans(string path)
        {
            var data = await lessonRepo.GetAll();

            var teachersUnique = DataUtil.GetTeachers(data);
            CreateTeacher(teachersUnique[0], path, data);
        }


        private void CreateTeacher(string teacher, string path, List<LessonModel> data)
        {
            using var doc = DocX.Create(Path.Combine(path, teacher + ".docx"));
            var lessons = data.Where(x => x.Teachers.Contains(teacher));
            var uniqueDisc = lessons.Select(x => x.Discipline).Distinct();
            var width = 1 + uniqueDisc.Count();
            var height = 26;
            var table = doc.AddTable(width, height);
            doc.InsertTable(table);
            doc.Save();
        }
    }
}
