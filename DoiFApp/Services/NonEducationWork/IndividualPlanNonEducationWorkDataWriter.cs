using DoiFApp.Enums;
using DoiFApp.Services.Data;
using DoiFApp.Utils.Extensions;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace DoiFApp.Services.NonEducationWork
{
    public class IndividualPlanNonEducationWorkDataWriter : IDataWriter<NonEducationWorkData>
    {
        public async Task<bool> Write(NonEducationWorkData data, string path)
        {
            if (!data.IsHolistic) return false;

            using var doc = DocX.Load(path);
            var tables = doc.Tables;

            var works = data.NonEducationWorks!;

            var firstSemester = works.Where(w => w.Semester == SemesterType.First);
            var secondSemester = works.Where(w => w.Semester == SemesterType.Second);

            await UpdateTableByType(tables, firstSemester, NonEducationWorkType.Methodic, data.IsRewrite);
            await UpdateTableByType(tables, secondSemester, NonEducationWorkType.Methodic, data.IsRewrite);

            await UpdateTableByType(tables, firstSemester, NonEducationWorkType.Scientic, data.IsRewrite);
            await UpdateTableByType(tables, secondSemester, NonEducationWorkType.Scientic, data.IsRewrite);

            await UpdateTableByType(tables, firstSemester, NonEducationWorkType.Moral, data.IsRewrite);
            await UpdateTableByType(tables, secondSemester, NonEducationWorkType.Moral, data.IsRewrite);

            await UpdateTableByType(tables, firstSemester, NonEducationWorkType.Foreignic, data.IsRewrite);
            await UpdateTableByType(tables, secondSemester, NonEducationWorkType.Foreignic, data.IsRewrite);

            await UpdateTableByType(tables, firstSemester, NonEducationWorkType.Other, data.IsRewrite);
            await UpdateTableByType(tables, secondSemester, NonEducationWorkType.Other, data.IsRewrite);

            doc.Save();
            return true;
        }

        private static Task UpdateTableByType(List<Table> tables,
            IEnumerable<DoiFApp.Data.Models.NonEducationWork> works,
            NonEducationWorkType type,
            bool isRewrite)
            => UpdateTable(tables[type.GetFirstId()], works.Where(w => w.Type == type), isRewrite);

        private static Task UpdateTable(Table table, IEnumerable<DoiFApp.Data.Models.NonEducationWork> works, bool isRewrite)
        {
            var i = 1;

            if (isRewrite)
                foreach (var row in table.Rows.Skip(1))
                    row.Remove();

            foreach (var work in works)
            {
                var row = table.InsertRow(i++);

                var id = row.Cells[0].Paragraphs[0];
                id.RemoveText(0);
                id.Append(table.Rows.Count.ToString());

                var text = row.Cells[1].Paragraphs[0];
                text.RemoveText(0);
                text.Append(work.Text);
            }

            return Task.CompletedTask;
        }
    }
}
