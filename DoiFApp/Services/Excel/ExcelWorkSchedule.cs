using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using DoiFApp.Utils;
using System.Text;
using System.Windows;

namespace DoiFApp.Services.Excel
{
    public class ExcelWorkSchedule(IRepo<LessonModel> lessonRepo) : IWorkSchedule
    {
        private readonly IRepo<LessonModel> lessonRepo = lessonRepo;

        private readonly Color saturdayColor = Color.FromArgb(255, 255, 192, 203); // low red
        private readonly Color sundayColor = Color.FromArgb(255, 255, 0, 0); // red

        private readonly Color koptevoColor = Color.FromArgb(255, 199, 199, 199); // grey
        private readonly Color volginoColor = Color.FromArgb(255, 255, 255, 111); // yellow
        private readonly Color otherColor = Color.FromArgb(255, 255, 175, 100); // orange
        private readonly Color transitionColor = Color.FromArgb(255, 100, 245, 100); // green
        private readonly Color withoutColor = Color.FromArgb(255, 0, 200, 200); // blue

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

            (int start, int end) teacherLine = (3, teachersUnique.Count + 2);

            // header
            for (var i = 0; i < teachersUnique.Count; i++)
            {
                var teacher = teachersUnique[i];
                var tcell = worksheet.Cells[1, teacherLine.start + i];
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
                    DrawLine(worksheet, tableVerticalIndex, teacherLine.end, saturdayColor);

                if (date.DayOfWeek == DayOfWeek.Sunday)
                    DrawLine(worksheet, tableVerticalIndex, teacherLine.end, sundayColor);

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
                            var lessionsCell = worksheet.Cells[tableVerticalIndex, teacherLine.start + j];
                            lessionsCell.Value += SwitchClassId(lesson.Time) + " ";

                            lessionsCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            lessionsCell.Style.Fill.BackgroundColor.SetColor(SelectCellColor(lesson.Auditoriums));
                        }
                    }
                }
            }

            AddNotes(teacherLine.end + 1, worksheet);

            // view
            DoSquare(worksheet, 1, 1, endDayNum - startDayNum + 1, teacherLine.end, (range) =>
            {
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                range.Style.Border.Top.Color.SetColor(Color.Black);
                range.Style.Border.Bottom.Color.SetColor(Color.Black);
                range.Style.Border.Left.Color.SetColor(Color.Black);
                range.Style.Border.Right.Color.SetColor(Color.Black);

                range.AutoFitColumns();
            });

            package.Save();
        }

        private void AddNotes(int startNoteX, ExcelWorksheet worksheet)
        {
            int startNoteY = 2, i = 0;

            AddNote(worksheet, startNoteY + i++, startNoteX + 1, koptevoColor, "Коптево");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, volginoColor, "Волгино");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, otherColor, "Др. площадки");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, withoutColor, "Без аудитории");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, transitionColor, "Переезд");

            i++;

            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "1", "09:00 - 10:30");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "2", "10:45 - 12:15");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "3", "12:30 - 14:00");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "3к", "14:15 - 15:45");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "4к", "15:00 - 16:30");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "4", "16:00 - 17:30");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "5", "16:40 - 18:10");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "6", "18:20 - 19:50");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "7", "20:00 - 21:30");
        }

        private void AddNote(ExcelWorksheet worksheet, int y, int x, object key, object value)
        {
            worksheet.Cells[y, x].Value = key;
            worksheet.Cells[y, x + 1].Value = value;
        }

        private void AddNote(ExcelWorksheet worksheet, int y, int x, Color key, object value)
        {
            worksheet.Cells[y, x].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[y, x].Style.Fill.BackgroundColor.SetColor(key);
            worksheet.Cells[y, x + 1].Value = value;

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
            => action(worksheet.Cells[$"{GetEndSymbol(hIndex1)}{vIndex1}:{GetEndSymbol(hIndex2)}{vIndex2}"]);

        private string GetEndSymbol(int hIndex)
        {
            var startByte = Encoding.ASCII.GetBytes("A");
            startByte[0] += Convert.ToByte(hIndex - 1);
            return Encoding.ASCII.GetString(startByte);
        }

        private Color SelectCellColor(List<string> auditoriums)
        {
            var auds = auditoriums.Where(a => a.Contains("к/"));

            if (!auds.Any())
                return withoutColor;

            var suites = auds.Select(a => a.Split("к/")[0]).Distinct();
            if (suites.Count() > 1)
                return transitionColor;

            return SwitchColorByAuditorium(suites.First());
        }

        private Color SwitchColorByAuditorium(string id)
            => id switch
            {
                "1" => volginoColor,
                "2" => volginoColor,
                "3" => volginoColor,
                "5" => koptevoColor,
                _ => otherColor,
            };
    }
}

