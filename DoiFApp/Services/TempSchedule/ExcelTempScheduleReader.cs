using DoiFApp.Data.Models;
using DoiFApp.Services.Data;
using OfficeOpenXml;

namespace DoiFApp.Services.TempSchedule
{
    public class ExcelTempScheduleReader : IDataReader<TempScheduleData>
    {
        public Task<TempScheduleData> Read(string path)
        {
            using var package = new ExcelPackage(path);
            var data = package.Workbook.Worksheets[0];

            var lessons = new List<LessonModel>();

            for (int i = 2; i < data.Dimension.End.Row; i++)
            {
                var inputData = new LessonModel
                {
                    Date = LessonModel.GetDateOnly(data.Cells[i, 1].GetCellValue<string>()),
                    Time = data.Cells[i, 2].GetCellValue<string>(),

                    Discipline = data.Cells[i, 3].GetCellValue<string>(),
                    LessionType = data.Cells[i, 4].GetCellValue<string>(),
                    Topic = data.Cells[i, 5].GetCellValue<string?>(),

                    Groups = data.Cells[i, 6].GetCellValue<string>().Split(',').Select(s => s.Trim()).ToList(),
                    Teachers = data.Cells[i, 7].GetCellValue<string>().Split('\n').Select(s => s.Trim()).ToList(),

                    Auditoriums = data.Cells[i, 8].GetCellValue<string>().Split(',').Select(s => s.Trim()).ToList(),

                    Wight = data.Cells[i, 9].GetCellValue<double>(),
                };

                var existingLessons = lessons.Where(l => l.Date == inputData.Date
                      && l.Time == inputData.Time
                      && l.Discipline == inputData.Discipline
                      && l.Groups == inputData.Groups);

                if (existingLessons.Any())
                    existingLessons.First().Teachers.Add(inputData.Teachers.First());
                else
                    lessons.Add(inputData);
            }
            return Task.FromResult(new TempScheduleData() { Lessons = lessons.Count > 0 ? lessons : null });
        }
    }
}
