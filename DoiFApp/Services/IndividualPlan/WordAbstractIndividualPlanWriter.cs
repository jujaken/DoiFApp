using DoiFApp.Data.Models;
using DoiFApp.Services.Data;
using DoiFApp.Utils;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace DoiFApp.Services.IndividualPlan
{
    public abstract class WordAbstractIndividualPlanWriter<T> : IDataWriter<T> where T : AbstractIndividualPlanData
    {
        public async Task<bool> Write(T data, string path)
        {
            if (!data.IsHolistic) return false;

            await FillPlan(data.TeacherModel!, path);
            return true;
        }

        protected async Task FillPlan(EducationTeacherModel teacher, string path)
        {
            using var doc = DocX.Load(path);
            var tables = doc.Tables;
            await UpdateTables(teacher, tables);
            doc.Save();
        }

        protected abstract Task UpdateTables(EducationTeacherModel teacher, List<Table> tables);

        protected async Task InsertData(Table table, List<EducationWorkModel> works, bool isSecond = false)
        {
            var activeWorks = works.Where(w => w.TypesAndHours.Sum(t => t.Value) != 0);

            for (int i = table.Rows.Count; i < activeWorks.Count() + 2 + (isSecond ? 1 : 0); i++)
                table.InsertRow(1);

            for (int i = table.Rows.Count; i > activeWorks.Count() + 2 + (isSecond ? 1 : 0); i--)
                table.RemoveRow(1);

            var dones = new double[50];
            var ri = 0;
            foreach (var work in activeWorks)
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

                dones[row.Cells.Count - 3] += workSum;
                row.Cells[^2].Paragraphs[0].RemoveText(0);
                row.Cells[^2].Paragraphs[0].Append(workSum.ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));

                var audiWorkSum = work.TypesAndHours
                    .Where(x => TableDataUtil.GetHeaders(TableDataUtil.Auditorium).Contains(x.Key))
                    .Sum(x => x.Value);

                dones[row.Cells.Count - 2] += audiWorkSum;
                row.Cells[^1].Paragraphs[0].RemoveText(0);
                row.Cells[^1].Paragraphs[0].Append(audiWorkSum.ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }

            if (isSecond)
                await InsertDones(table, dones, 2);
            else
                await InsertDones(table, dones);

            table.Design = TableDesign.TableGrid;
        }

        protected Task InsertDones(Table tableCurrent, double[] dones, int endIndex = 1)
        {
            var resultRow = tableCurrent.Rows[^endIndex];
            for (int i = 1; i < resultRow.Cells.Count; i++)
            {
                var cell = resultRow.Cells[i];
                cell.Paragraphs[0].RemoveText(0);
                cell.Paragraphs[0].Append(dones[i - 1].ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }
            return Task.CompletedTask;
        }

        protected Task InsertDones(Table tableCurrent, Table tableFrom)
        {
            var yearRow = tableCurrent.Rows[^1];
            var donesCurrent = GetDonesFromTable(tableCurrent, 2);
            var donesFrom = GetDonesFromTable(tableFrom, 1);
            for (int i = 1; i < yearRow.Cells.Count; i++)
            {
                var cell = yearRow.Cells[i];
                cell.Paragraphs[0].RemoveText(0);
                cell.Paragraphs[0].Append((donesCurrent[i - 1] + donesFrom[i - 1]).ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")));
            }
            return Task.CompletedTask;
        }

        protected double[] GetDonesFromTable(Table table, int endIndex)
        {
            var donesRow = table.Rows[^endIndex];
            var dones = new double[donesRow.Cells.Count - 1];
            for (int i = 0; i < dones.Length; i++)
                dones[i] = Convert.ToDouble(donesRow.Cells[i + 1].Paragraphs[0].Text, System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            return dones;
        }
    }
}
