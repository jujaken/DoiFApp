using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using System.IO;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace DoiFApp.Services.Word
{
    public class WordIndividualPlanWriter(IRepo<EducationTeacherModel> teacherRepo) : IIndividualPlanWriter
    {
        private readonly IRepo<EducationTeacherModel> teacherRepo = teacherRepo;

        private const string SimpleDocName = "Resources/individualplansimple.docx";

        public async Task MakePlans(string path)
        {
            Directory.CreateDirectory(path);
            (await teacherRepo.GetAll()).ForEach(async teacher =>
            {
                await CreateTeacher(teacher, Path.Combine(path, teacher.Name + ".docx"));
            });
        }

        private static Task CreateTeacher(EducationTeacherModel teacher, string fileName)
        {
            using var simpleDoc = DocX.Load(SimpleDocName);
            simpleDoc.SaveAs(fileName);
            simpleDoc.Dispose();

            using var doc = DocX.Load(fileName);
            var tables = doc.Tables;
            UpdateTables(teacher, tables);
            doc.Save();

            return Task.CompletedTask;
        }

        private static void UpdateTables(EducationTeacherModel teacher, List<Table> tables)
        {
            // уч работа

            // план
            // 1
            InsertData(tables[0], teacher.Works1);
            // 2
            InsertData(tables[1], teacher.Works2);

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

        private static void InsertData(Table table, List<EducationWorkModel> works)
        {
            foreach (var work in works)
            {
                var row = table.InsertRow(1);
                row.Cells[0].Paragraphs[0].Append(work.Name);
                for (int i = 1; i < row.Cells.Count - 2; i++)
                {
                    var cell = row.Cells[i];
                    var item = work.TypesAndHours[i - 1];
                    var value = item.Value.ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")) ?? "0.0";
                    cell.Paragraphs[0].Append(value);
                }
            }

            table.Design = TableDesign.TableGrid;
        }
    }
}
