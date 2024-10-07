using DoiFApp.Data.Models;
using DoiFApp.Services.Data;
using OfficeOpenXml;

namespace DoiFApp.Services.Schedule
{
    public class ExcelScheduleReader : IDataReader<ScheduleData>
    {
        public Task<ScheduleData> Read(string path)
        {
            using var package = new ExcelPackage(path);
            var data = package.Workbook.Worksheets[1];

            var lessons = new List<LessonModel>();

            for (int i = 5; i < data.Dimension.End.Row; i++)
            {
                if (data.Cells[i, 1].GetCellValue<string>() == null)
                    break;

                var inputData = new LessonModel
                {
                    Date = LessonModel.GetDateOnly(data.Cells[i, 1].GetCellValue<string>()),
                    Time = data.Cells[i, 3].GetCellValue<string>(),

                    Discipline = data.Cells[i, 5].GetCellValue<string>(),
                    LessionType = data.Cells[i, 6].GetCellValue<string>(),
                    Topic = data.Cells[i, 7].GetCellValue<string?>(),

                    Groups = data.Cells[i, 4].GetCellValue<string>()
                        .Split(',').Select(s => s.Trim()).ToList(),
                    Teachers = data.Cells[i, 8].GetCellValue<string>()
                        .Split('\n').Select(s => s.Trim()).ToList(),

                    Auditoriums = (data.Cells[i, 9].GetCellValue<string?>() ?? "без аудитории")
                        .Split(',').Select(s => s.Trim()).ToList()
                };

                if (inputData.LessionType.Contains("зач", StringComparison.CurrentCultureIgnoreCase)
                    || inputData.LessionType.Contains("экз", StringComparison.CurrentCultureIgnoreCase))
                {
                    var lesson = lessons.Where(lesson => lesson.Date == inputData.Date
                        && lesson.Topic == inputData.Topic
                        && lesson.Discipline == inputData.Discipline).FirstOrDefault();

                    if (lesson != null)
                    {
                        lesson.Wight += 2;
                        continue;
                    }
                }
            }

            return Task.FromResult(new ScheduleData() { Lessons = lessons.Count > 0 ? lessons : null });
        }
    }
}
