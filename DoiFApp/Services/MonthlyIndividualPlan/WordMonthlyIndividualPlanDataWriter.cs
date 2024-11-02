using DoiFApp.Data.Models;
using DoiFApp.Services.Data;
using DoiFApp.Utils;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace DoiFApp.Services.MonthlyIndividualPlan
{
    public class WordMonthlyIndividualPlanDataWriter : IDataWriter<MonthlyIndividualPlanData>
    {
        public static string[] Convertions => TableDataUtil.GetHeaders(TableDataUtil.InputCommonTableHeaders).ToArray();

        public async Task<bool> Write(MonthlyIndividualPlanData data, string path)
        {
            if (!data.IsHolistic) return false;

            await FillPlan(data, path);
            return true;
        }

        protected static async Task FillPlan(MonthlyIndividualPlanData data, string path)
        {
            using var doc = DocX.Load(path);
            var tables = doc.Tables;
            await InsertData(tables[4], data.Lessons!, data.Converters!);
            doc.Save();
        }

        protected static async Task InsertData(Table table,
            IEnumerable<LessonModel> lessons,
            IEnumerable<LessonTypeConverter> converters)
        {
            // с августа по декабрь
            for (int i = 8; i < 13; i++)
                await FillRow(table.Rows[i - 7], lessons.Where(l => l.Date.Month == i), converters);
            await FillRow(table.Rows[6], lessons.Where(l => 8 <= l.Date.Month && l.Date.Month <= 12), converters);
            // с января по июль
            for (int i = 1; i < 8; i++)
                await FillRow(table.Rows[i + 6], lessons.Where(l => l.Date.Month == i), converters);
            await FillRow(table.Rows[14], lessons.Where(l => l.Date.Month <= 7), converters);
            // итог
            await FillRow(table.Rows[15], lessons, converters);
        }

        protected static Task FillRow(Row row,
            IEnumerable<LessonModel> lessons,
            IEnumerable<LessonTypeConverter> converters)
        {
            for (int i = 0; i < Convertions.Length; i++)
            {
                var converter = converters.Where(c => c.Convertion == Convertions[i]).FirstOrDefault();
                if (converter == null) continue;

                var sum = lessons.Where(l => l.LessionType == converter.TypeName).Sum(l => l.Wight);
                var value = sum.ToString("0.0", System.Globalization.CultureInfo.GetCultureInfo("en-US")) ?? "0.0";
                var cell = row.Cells[i + 1];
                cell.Paragraphs[0].RemoveText(0);
                cell.Paragraphs[0].Append(value);
            }
            return Task.CompletedTask;
        }
    }
}
