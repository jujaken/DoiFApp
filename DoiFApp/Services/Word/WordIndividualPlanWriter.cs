using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Utils;
using System.IO;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace DoiFApp.Services.Word
{
    public class WordIndividualPlanWriter(IRepo<LessonModel> lessonRepo) : IIndividualPlanWriter
    {
        private readonly IRepo<LessonModel> lessonRepo = lessonRepo;
        private readonly string simpleDocName = "Resources/individualplansimple.docx";

        public async Task MakePlans(string path)
        {
            var data = await lessonRepo.GetAll();

            var teachersUnique = DataUtil.GetTeachers(data);
            CreateTeacher(teachersUnique[0], path, data);
        }


        private void CreateTeacher(string teacher, string path, List<LessonModel> data)
        {
            using var simpleDoc = DocX.Load(simpleDocName);
            var fileName = Path.Combine(path, teacher + ".docx");
            simpleDoc.SaveAs(fileName);
            simpleDoc.Dispose();

            using var doc = DocX.Load(fileName);
            var lessons = data.Where(x => x.Teachers.Contains(teacher));
            var uniqueDisc = lessons.Select(x => x.Discipline).Distinct();
            var tables = doc.Tables;
            UpdateTables(teacher, tables, data);
            doc.Save();
        }

        private static void UpdateTables(string teacher, List<Table> tables, List<LessonModel> data)
        {
            // уч работа

            var disciplines = data.Where(l => l.Teachers.Contains(teacher)).Select(l => l.Discipline).ToList();
            disciplines.Sort();
            disciplines.Reverse();
            // план
            // 1
            var table = tables[0];
            disciplines.ForEach(d =>
            {
                var row = table.InsertRow(1);
                row.Cells[0].Paragraphs[0].Append(d);
                foreach (var cell in row.Cells.Skip(1))
                {
                    cell.Paragraphs[0].Append("0.0");
                }
            });
            table.Design = TableDesign.TableGrid;
            // 2

            // факт
            // 1

            // 2

            // ежемесячные

            // методическая
            // 1

            // 2

            // наука
            // 1

            // 2

            // воспитательная
            // 1

            // 2

            // иностранцы
            // 1

            // 2

            // другие
            // 1

            // 2
        }
    }
}
