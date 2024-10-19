using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using DoiFApp.Utils;
using System.Text;
using DoiFApp.Services.Data;

namespace DoiFApp.Services.Workload
{
    public class ExcelWorkloadWriter : IDataWriter<WorkloadData>
    {
        public Task<bool> Write(WorkloadData data, string path)
        {
            if (!data.IsHolistic) return Task.FromResult(false);

            if (File.Exists(path))
                File.Delete(path);

            using var package = new ExcelPackage(path);
            var worksheet = package.Workbook.Worksheets.Add("График");

            var teachersUnique = DataUtil.GetTeachers(data.Lessons!);

            (int start, int end) = (3, teachersUnique.Count + 2);

            // header
            for (var i = 0; i < teachersUnique.Count; i++)
            {
                var teacher = teachersUnique[i];
                var tcell = worksheet.Cells[1, start + i];
                tcell.Value = teacher;
                tcell.Style.Border.BorderAround(ExcelBorderStyle.Hair);
                tcell.Style.TextRotation = 180;
            }

            var startDayNum = data.Lessons!.First().Date.DayNumber;
            var endDayNum = data.Lessons!.Last().Date.DayNumber;

            for (var i = startDayNum; i < endDayNum; i++)
            {
                var date = DateOnly.FromDayNumber(i);
                var tableVerticalIndex = 2 + i - startDayNum;
                worksheet.Cells[tableVerticalIndex, 1].Value = date;

                var curDay = DateUtil.SwitchDayOfWeek(date.DayOfWeek);
                worksheet.Cells[tableVerticalIndex, 2].Value = curDay;

                if (date.DayOfWeek == DayOfWeek.Saturday)
                    DrawRow(worksheet, tableVerticalIndex, 2, end, WorkloadHelper.SaturdayColor);

                if (date.DayOfWeek == DayOfWeek.Sunday)
                    DrawRow(worksheet, tableVerticalIndex, 2, end, WorkloadHelper.SundayColor);

                var lessons = data.Lessons!.Where(l => l.Date == date);

                if (lessons == null)
                    continue;

                foreach (var lesson in lessons)
                {
                    for (var j = 0; j < teachersUnique.Count; j++)
                    {
                        var teacher = teachersUnique[j];
                        if (lesson.Teachers.Contains(teacher))
                        {
                            var lessionsCell = worksheet.Cells[tableVerticalIndex, start + j];
                            lessionsCell.Value += SwitchClassId(lesson.Time) + " ";

                            lessionsCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            lessionsCell.Style.Fill.BackgroundColor.SetColor(WorkloadHelper.SelectCellColor(lesson.Auditoriums));
                        }
                    }
                }
            }

            AddNotes(end + 1, worksheet);

            // view
            DoSquare(worksheet, 1, 1, endDayNum - startDayNum + 1, end, (range) =>
            {
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                range.Style.Border.Top.Color.SetColor(Color.Black);
                range.Style.Border.Bottom.Color.SetColor(Color.Black);
                range.Style.Border.Left.Color.SetColor(Color.Black);
                range.Style.Border.Right.Color.SetColor(Color.Black);

                range.AutoFitColumns(worksheet.DefaultColWidth / 2);
            });

            package.Save();

            return Task.FromResult(true);
        }

        private static void AddNotes(int startNoteX, ExcelWorksheet worksheet)
        {
            int startNoteY = 2, i = 0;

            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.KoptevoColor, "Коптево");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.VolginoColor, "Волгино");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.OtherColor, "Др. площадки");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.WithoutColor, "Без аудитории");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.TransitionColor, "Переезд");

            i++;

            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "1", "09:00 - 10:30");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "2", "10:45 - 12:15");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "2к", "13:15 - 14:45");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "3", "12:30 - 14:00");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "3к", "14:15 - 15:45");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "4к", "15:00 - 16:30");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "4", "16:00 - 17:30");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "5", "16:40 - 18:10");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "6", "18:20 - 19:50");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, "7", "20:00 - 21:30");
        }

        private static void AddNote(ExcelWorksheet worksheet, int y, int x, object key, object value)
        {
            worksheet.Cells[y, x].Value = key;
            worksheet.Cells[y, x + 1].Value = value;
        }

        private static void AddNote(ExcelWorksheet worksheet, int y, int x, Color key, object value)
        {
            worksheet.Cells[y, x].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[y, x].Style.Fill.BackgroundColor.SetColor(key);
            worksheet.Cells[y, x + 1].Value = value;
        }

        private static string SwitchClassId(string time)
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
                _ => "n"
            };

        private static void DrawRow(ExcelWorksheet worksheet, int y, int x1, int x2, Color color)
        {
            var range = worksheet.Cells[y, x1, y, x2];

            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
            range.Style.Fill.BackgroundColor.SetColor(color);
        }

        private static void DoSquare(ExcelWorksheet worksheet, int y1, int x1, int y2, int x2, Action<ExcelRange> action)
            => action(worksheet.Cells[y1, x1, y2, x2]);
    }
}

