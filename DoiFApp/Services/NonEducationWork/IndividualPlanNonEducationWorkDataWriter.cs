using DoiFApp.Services.Data;
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

            var firstSemester = works.Where(w => w.Semester == DoiFApp.Data.Models.NonEducationWorkSemesterType.First);
            var secondSemester = works.Where(w => w.Semester == DoiFApp.Data.Models.NonEducationWorkSemesterType.Second);

            // todo UpdateTable...

            doc.Save();
            return true;
        }

        public Task UpdateTable(Table table, IEnumerable<DoiFApp.Data.Models.NonEducationWork> works)
        {
            var i = 1;
            foreach (var work in works)
            {
                var paragraph = table.InsertRow(i++).Cells[0].Paragraphs[0];
                paragraph.RemoveText(0);
                paragraph.Append(work.Text);
            }
            return Task.CompletedTask;
        }
    }
}
