using DoiFApp.Data.Models;
using DoiFApp.Utils;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace DoiFApp.Services.Word
{
    public class WordIndividualPlanWriter : IIndividualPlanWriter
    {
        public Task FillPlan(EducationTeacherModel teacher, string path)
        {
            using var doc = DocX.Load(path);
            var tables = doc.Tables;
            UpdateTables(teacher, tables);
            doc.Save();
            return Task.CompletedTask;
        }

        private static void UpdateTables(EducationTeacherModel teacher, List<Table> tables)
        {
            // ПЛАНИРУЕМАЯ НА 1 ПОЛУГОДИЕ
            var w1Dones = InsertData(tables[0], teacher.Works1);
            // ПЛАНИРУЕМАЯ НА 2 ПОЛУГОДИЕ
            InsertData(tables[1], teacher.Works2, w1Dones);
            // ФАКТИЧЕСКИ ВЫПОЛНЕННАЯ В 1 ПОЛУГОДИИ
            // ФАКТИЧЕСКИ ВЫПОЛНЕННАЯ В 2 ПОЛУГОДИИ
            // ЕЖЕМЕСЯЧНЫЙ УЧЁТ ВЫПОЛНЕНИЯ УЧЕБНОЙ НАГРУЗКИ 

        }

        private static double[] InsertData(Table table, List<EducationWorkModel> works, double[]? lastDones = null)
        {
            for (int i = table.Rows.Count - (lastDones == null ? 2 : 3); i < works.Count; i++)
                table.InsertRow(1);

            var dones = new double[50];
            var ri = 0;
            foreach (var work in works)
            {
                var row = table.Rows[++ri];
                row.Cells[0].Paragraphs[0].RemoveText(0);
                row.Cells[0].Paragraphs[0].Append(work.Name);
                for (int i = 1; i < row.Cells.Count - 2; i++)
                {
                    var cell = row.Cells[i];
                    var item = work.TypesAndHours[i - 1];
                    var value = item.Value.ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")) ?? "0.0";
                    dones[i - 1] += work.TypesAndHours[i - 1].Value;
                    cell.Paragraphs[0].RemoveText(0);
                    cell.Paragraphs[0].Append(value);
                }

                var workSum = work.TypesAndHours
                    .Sum(x => x.Value);

                dones[row.Cells.Count - 3] = workSum;
                row.Cells[^2].Paragraphs[0].RemoveText(0);
                row.Cells[^2].Paragraphs[0].Append(workSum.ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));

                var audiWorkSum = work.TypesAndHours
                    .Where(x => TableDataUtil.GetEquivalent(x.Key) != null)
                    .Sum(x => x.Value);

                dones[row.Cells.Count - 2] = workSum;
                row.Cells[^1].Paragraphs[0].RemoveText(0);
                row.Cells[^1].Paragraphs[0].Append(audiWorkSum.ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }

            var lastRow = table.Rows[^(lastDones == null ? 1 : 2)];
            for (int i = 1; i < lastRow.Cells.Count; i++)
            {
                var cell = lastRow.Cells[i];
                cell.Paragraphs[0].RemoveText(0);
                cell.Paragraphs[0].Append(dones[i - 1].ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }

            if (lastDones != null)
            {
                var yearRow = table.Rows[^1];
                for (int i = 1; i < lastRow.Cells.Count; i++)
                {
                    var cell = lastRow.Cells[i];
                    cell.Paragraphs[0].RemoveText(0);
                    cell.Paragraphs[0].Append((dones[i - 1] + lastDones[i - 1]).ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                }
            }

            table.Design = TableDesign.TableGrid;
            return dones;
        }
    }
}
