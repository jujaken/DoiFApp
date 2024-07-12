using DoiFApp.Models;
using System.IO;
using System.Windows;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace DoiFApp.Services.Word
{
    public class WordIndividualPlanWriter : IIndividualPlanWriter
    {
        private readonly string simpleDocName = "Resources/individualplansimple.docx";

        public Task MakePlans(List<EducationTeacherModel> data, string path)
        {
            data.ForEach(async teacher =>
            {
                await CreateTeacher(teacher, Path.Combine(path, teacher.Name + ".docx"));
            });

            return Task.CompletedTask;
        }


        private Task CreateTeacher(EducationTeacherModel teacher, string fileName)
        {
            using var simpleDoc = DocX.Load(simpleDocName);
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
                foreach (var item in work.TypesAndHours)
                {
                    var row = table.InsertRow(1);
                    row.Cells[0].Paragraphs[0].Append(work.Name);
                    foreach (var cell in row.Cells.Skip(1))
                        cell.Paragraphs[0].Append(item.Value.ToString("0.00", System.Globalization.CultureInfo.GetCultureInfo("en-US")) ?? "0.0");
                }
            table.Design = TableDesign.TableGrid;
        }
    }
}
