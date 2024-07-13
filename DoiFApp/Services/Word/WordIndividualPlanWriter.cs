using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using System.IO;
using Xceed.Document.NET;
using Xceed.Words.NET;
using static OfficeOpenXml.ExcelErrorValue;

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
        private const string AuditoriumLoadStr = "практические занятия в подгруппе\tучения, д/и, круглый стол\tконсультации перед экзаменами\tзащита практики\tзачет устный\tвступительные испытания\tэкзамены\tгосударственные экзамены\tвступительные и кандитатские экзамены (адъюнктура)";
        private static readonly IEnumerable<string> auditoriumLoad = AuditoriumLoadStr.Split('\t', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries);

        private static void InsertData(Table table, List<EducationWorkModel> works)
        {
            var doneList = new double[50];
            foreach (var work in works)
            {
                var row = table.InsertRow(1);
                row.Cells[0].Paragraphs[0].Append(work.Name);
                for (int i = 1; i < row.Cells.Count - 2; i++)
                {
                    var cell = row.Cells[i];
                    var item = work.TypesAndHours[i - 1];
                    var value = item.Value.ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")) ?? "0.0";
                    doneList[i - 1] += work.TypesAndHours[i - 1].Value;
                    cell.Paragraphs[0].Append(value);
                }

                var workSum = work.TypesAndHours
                    .Sum(x => x.Value);
                doneList[row.Cells.Count - 3] = workSum;
                row.Cells[^2].Paragraphs[0].Append(workSum.ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));

                var audiWorkSum = work.TypesAndHours
                    .Where(x => auditoriumLoad.Contains(x.Key))
                    .Sum(x => x.Value);
                doneList[row.Cells.Count - 2] = workSum;
                row.Cells[^1].Paragraphs[0].Append(audiWorkSum.ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }

            var lastRow = table.Rows[^1];
            for (int i = 1; i < lastRow.Cells.Count; i++)
            {
                var cell = lastRow.Cells[i];
                cell.Paragraphs[0].ReplaceText(new StringReplaceTextOptions()
                {
                    SearchValue = "0.0",
                    NewValue = doneList[i - 1].ToString("0.0",
                        System.Globalization.CultureInfo.GetCultureInfo("en-US")),
                });
            }

            table.Design = TableDesign.TableGrid;
        }
    }
}
