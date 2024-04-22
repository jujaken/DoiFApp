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
    public class ExcelWorkSchedule(IRepo<LessonModel> lessonRepo) : IWorkSchedule
    {
        private readonly IRepo<LessonModel> lessonRepo = lessonRepo;

        private readonly Color saturdayColor = Color.FromArgb(255, 255, 192, 203);
        private readonly Color sundayColor = Color.FromArgb(255, 255, 0, 0);

        private readonly Color koptevoColor = Color.FromArgb(255, 211, 211, 211);
        private readonly Color volginoColor = Color.FromArgb(255, 169, 127, 211);
        private readonly Color otherColor = Color.FromArgb(255, 245, 245, 222);

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
                    DrawLine(worksheet, tableVerticalIndex, teachersUnique.Count, saturdayColor);

                if (date.DayOfWeek == DayOfWeek.Sunday)
                    DrawLine(worksheet, tableVerticalIndex, teachersUnique.Count, sundayColor);

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

                            lessionsCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            if (lesson.Auditoriums.First().Contains("к"))
                            {
                                var color = SwitchColorByAuditorium(lesson.Auditoriums.First().Split("к/")[0]);
                                lessionsCell.Style.Fill.BackgroundColor.SetColor(color);
                            }
                        }
                    }
                }
            }

            AddNote(teachersUnique, worksheet);

            // view
            DoSquare(worksheet, 1, 1, endDayNum - startDayNum, teachersUnique.Count - 2, (range) =>
            {
                range.AutoFitColumns();
            });

            package.Save();
        }

        private void AddNote(List<string> teachersUnique, ExcelWorksheet worksheet)
        {
            var startNoteX = teachersUnique.Count + 4;
            var startNoteY = 2;

            worksheet.Cells[startNoteY, startNoteX].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[startNoteY, startNoteX].Style.Fill.BackgroundColor.SetColor(koptevoColor);
            worksheet.Cells[startNoteY, startNoteX + 1].Value = "Коптево";

            worksheet.Cells[startNoteY + 1, startNoteX].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[startNoteY + 1, startNoteX].Style.Fill.BackgroundColor.SetColor(volginoColor);
            worksheet.Cells[startNoteY + 1, startNoteX + 1].Value = "Волгино";

            worksheet.Cells[startNoteY + 2, startNoteX].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[startNoteY + 2, startNoteX].Style.Fill.BackgroundColor.SetColor(otherColor);
            worksheet.Cells[startNoteY + 2, startNoteX + 1].Value = "Др. площадки";


            worksheet.Cells[startNoteY + 6, startNoteX].Value = "1";
            worksheet.Cells[startNoteY + 6, startNoteX + 1].Value = "09:00 - 10:30";

            worksheet.Cells[startNoteY + 7, startNoteX].Value = "2";
            worksheet.Cells[startNoteY + 7, startNoteX + 1].Value = "10:45 - 12:15";

            worksheet.Cells[startNoteY + 8, startNoteX].Value = "3";
            worksheet.Cells[startNoteY + 8, startNoteX + 1].Value = "12:30 - 14:00";

            worksheet.Cells[startNoteY + 9, startNoteX].Value = "3к";
            worksheet.Cells[startNoteY + 9, startNoteX + 1].Value = "14:15 - 15:45";

            worksheet.Cells[startNoteY + 10, startNoteX].Value = "4к";
            worksheet.Cells[startNoteY + 10, startNoteX + 1].Value = "15:00 - 16:30";

            worksheet.Cells[startNoteY + 11, startNoteX].Value = "4";
            worksheet.Cells[startNoteY + 11, startNoteX + 1].Value = "16:00 - 17:30";

            worksheet.Cells[startNoteY + 12, startNoteX].Value = "5";
            worksheet.Cells[startNoteY + 12, startNoteX + 1].Value = "16:40 - 18:10";

            worksheet.Cells[startNoteY + 13, startNoteX].Value = "6";
            worksheet.Cells[startNoteY + 13, startNoteX + 1].Value = "18:20 - 19:50";

            worksheet.Cells[startNoteY + 14, startNoteX].Value = "7";
            worksheet.Cells[startNoteY + 14, startNoteX + 1].Value = "20:00 - 21:30";

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

            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
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

        private Color SwitchColorByAuditorium(string id)
            => id switch
            {
                "1" => volginoColor,
                "2" => volginoColor,
                "3" => volginoColor,
                "4" => koptevoColor,
                "5" => otherColor,
                "6" => otherColor,
                "7" => otherColor,
                "8" => otherColor,
                "9" => otherColor,
                "10" => otherColor,
                "11" => otherColor,
                _ => throw new Exception()
            };
    }
}
