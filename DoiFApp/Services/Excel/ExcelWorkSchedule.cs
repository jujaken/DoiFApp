
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Data;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using DoiFApp.Utils;
using System.Text;

namespace DoiFApp.Services.Excel
{
    public class ExcelWorkSchedule(AppDbContext context, IRepo<LessonModel> lessonRepo, ICaseComparator caseComparator) : IWorkSchedule
    {
        private readonly AppDbContext context = context;
        private readonly IRepo<LessonModel> lessonRepo = lessonRepo;
        private readonly ICaseComparator caseComparator = caseComparator;

        public async Task Write(string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            using var package = new ExcelPackage(path);
            var worksheet = package.Workbook.Worksheets.Add("График");

            var data = await lessonRepo.GetAll();

            // unique teachers
            var teachersNotUnique = data.Select(l => l.Teachers);
            var teachersUnique = new List<string>();

            foreach (var teachers in teachersNotUnique)
                foreach (var teacher in teachers)
                    if (!teachersUnique.Contains(teacher))
                        teachersUnique.Add(teacher);

            teachersUnique.Sort();

            // header
            for (var i = 0; i < teachersUnique.Count; i++)
            {
                var teacher = teachersUnique[i];
                var tcell = worksheet.Cells[1, 2 + i];
                tcell.Value = teacher;
                tcell.Style.Border.BorderAround(ExcelBorderStyle.Hair);
                tcell.Style.TextRotation = 180;
            }

            var startDayNum = data.First().Date.DayNumber;
            var endDayNum = data.Last().Date.DayNumber;

            for (var i = startDayNum; i < endDayNum; i++)
            {
                var date = DateOnly.FromDayNumber(i);
                var tableVerticalIndex = 2 + i - startDayNum;
                worksheet.Cells[tableVerticalIndex, 1].Value = date;

                var curDay = DateUtil.SwitchDayOfWeek(date.DayOfWeek);
                worksheet.Cells[tableVerticalIndex, 2].Value = curDay;

                if (date.DayOfWeek == DayOfWeek.Saturday)
                    DrawLine(worksheet, tableVerticalIndex, teachersUnique.Count, Color.FromArgb(255, 150, 130, 130));

                if (date.DayOfWeek == DayOfWeek.Sunday)
                    DrawLine(worksheet, tableVerticalIndex, teachersUnique.Count, Color.FromArgb(255, 150, 10, 10));

                var lessons = data.Where(l => l.Date == date);

                if (lessons == null)
                    continue;

                foreach (var lesson in lessons)
                {
                    for (var j = 0; j < teachersUnique.Count; j++)
                    {
                        var teacher = teachersUnique[j];
                        if (lesson.Teachers.Contains(teacher))
                        {
                            var lessionsCell = worksheet.Cells[tableVerticalIndex, 2 + j];
                            lessionsCell.Value += SwitchClassId(lesson.Time) + " ";
                        }
                    }
                }
            }

            DoSquare(worksheet, 1, 1, endDayNum - startDayNum, teachersUnique.Count - 2, (range) =>
            {
                range.AutoFitColumns();
            });

            package.Save();
        }

        private string SwitchClassId(string time)
            => time switch
            {
                "09:00 - 10:30" => "1",
                "10:45 - 12:15" => "2",
                "12:30 - 14:00" => "3",
                "14:15 - 15:45" => "3к",
                "15:00 - 16:30" => "4к",
                "16:00 - 17:30" => "4",
                "16:40 - 18:10" => "5",
                "18:20 - 19:50" => "6",
                "20:00 - 21:30" => "7",
                _ => throw new Exception()
            };

        private void DrawLine(ExcelWorksheet worksheet, int vIndex, int hIndex, Color color)
        {
            var rangeString = $"A{vIndex}:{GetEndSymbol(hIndex)}{vIndex}";
            var range = worksheet.Cells[rangeString];

            range.Style.Fill.PatternType = ExcelFillStyle.DarkGrid;
            range.Style.Fill.PatternColor.SetColor(color);
            range.Style.Fill.BackgroundColor.SetColor(color);
        }

        private void DoSquare(ExcelWorksheet worksheet, int vIndex1, int hIndex1, int vIndex2, int hIndex2, Action<ExcelRange> action)
        {
            var rangeString = $"{GetEndSymbol(hIndex1)}{vIndex1}:{GetEndSymbol(hIndex2)}{vIndex2}";
            var range = worksheet.Cells[rangeString];
            action(range);
        }

        private string GetEndSymbol(int hIndex)
        {
            var startByte = Encoding.ASCII.GetBytes("A");
            startByte[0] += Convert.ToByte(hIndex + 2);
            return Encoding.ASCII.GetString(startByte);
        }
    }
}
