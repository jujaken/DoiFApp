using DoiFApp.Data.Models;
using DoiFApp.Services.Data;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace DoiFApp.Services.MonthlyIndividualPlan
{
    public class WordMonthlyIndividualPlanDataWriter : IDataWriter<MonthlyIndividualPlanData>
    {
        public async Task<bool> Write(MonthlyIndividualPlanData data, string path)
        {
            if (!data.IsHolistic) return false;

            await FillPlan(data, path);
            return true;
        }

        protected async Task FillPlan(MonthlyIndividualPlanData data, string path)
        {
            using var doc = DocX.Load(path);
            var tables = doc.Tables;
            await InsertData(tables[4], data.Lessons!);
            doc.Save();
        }

        protected async Task InsertData(Table table, IEnumerable<LessonModel> lessons)
        {
            // с августа по декабрь
            for (int i = 8; i < 13; i++)
                await FillRow(table.Rows[i - 7], i, lessons);
            // с января по июль
            for (int i = 1; i < 9; i++)
                await FillRow(table.Rows[i + 6], i, lessons);
        }

        protected Task FillRow(Row row, int month, IEnumerable<LessonModel> lessons)
        {
            lessons = lessons.Where(l => l.Date.Month == month);
            // todo
            return Task.CompletedTask;
        }
    }
}
