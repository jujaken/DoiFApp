using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using DoiFApp.Utils;
using DoiFApp.Services.Data;
using DoiFApp.Config;
using DoiFApp.Data.Models;

namespace DoiFApp.Services.Workload
{
    public class ExcelWorkloadWriter(IAppConfigService cfgService) : IDataWriter<WorkloadData>
    {
        public async Task<bool> Write(WorkloadData data, string path)
        {
            var settings = await cfgService.Get(App.SettingsPath) ?? throw new ArgumentNullException(nameof(cfgService));
            var settingsCategory = settings.ConfigColorCategories.Where(c => c.Tittle == WorkloadHelper.CategoryName).FirstOrDefault() ?? throw new ArgumentNullException(nameof(settings));

            if (!data.IsHolistic || data.Lessons == null) return false;

            if (File.Exists(path))
                File.Delete(path);

            using var package = new ExcelPackage(path);
            var worksheet = package.Workbook.Worksheets.Add("График");

            var teachersUnique = DataUtil.GetTeachers(data.Lessons);

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

            var index = 1;

            var startDate = data.Lessons.First().Date;
            var endDate = data.Lessons.Last().Date;

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                index++;

                worksheet.Cells[index, 1].Value = $"{date:dd.MM.yy}";
                worksheet.Cells[index, 2].Value = DateUtil.SwitchDayOfWeek(date.DayOfWeek);

                if (date.DayOfWeek == DayOfWeek.Saturday)
                    DrawRow(worksheet, index, 2, end, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.SaturdayColorName));
                else if (date.DayOfWeek == DayOfWeek.Sunday)
                    DrawRow(worksheet, index, 2, end, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.SundayColorName));

                var curDayLessons = data.Lessons.Where(l => l.Date == date);
             
                for (var j = 0; j < teachersUnique.Count; j++)
                {
                    var teacher = teachersUnique[j];
                    var lessonsCell = worksheet.Cells[index, start + j];
                    var curTeacherCurDayLessons = curDayLessons.Where(l => l.Teachers.Contains(teacher));
                    if (!curTeacherCurDayLessons.Any()) continue;

                    foreach (var lesson in curTeacherCurDayLessons)
                        lessonsCell.Value += SwitchClassId(lesson) + " ";

                    lessonsCell.Style.Fill.PatternType = ExcelFillStyle.Solid;

                    var buildings = curTeacherCurDayLessons
                        .Select(l => l.Auditoriums)
                        .Select(WorkloadHelper.GetBuildings)
                        .SelectMany(list => list)
                        .Distinct()
                        .ToList();

                    buildings.Remove("Без корпуса");

                    if (buildings.Count == 0)
                    {
                        lessonsCell.Style.Fill.BackgroundColor
                            .SetColor(WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.WithoutColorName));
                        continue;
                    }

                    if (buildings.Count == 1)
                    {
                        lessonsCell.Style.Fill.BackgroundColor
                             .SetColor(WorkloadHelper.SwitchColorByBuilding(settingsCategory, buildings[0]));
                        continue;
                    }

                    lessonsCell.Style.Fill.BackgroundColor
                        .SetColor(WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.TransitionColorName));
                }
            }

            AddNotes(end + 1, worksheet, settingsCategory);
            DoSquare(worksheet, 1, 1, index, end, (range) =>
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

            return true;
        }

        private static void AddNotes(int startNoteX, ExcelWorksheet worksheet, ConfigColorCategory settingsCategory)
        {
            int startNoteY = 2, i = 0;

            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.KoptevoColorName), "Коптево");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.VolginoColorName), "Волгина");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.BobruiskayaColorName), "Бобруйская");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.GlavnayaColorName), "Главная 86");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.OkrujnoyColorName), "Окружная");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.KolskayaColorName), "Кольская");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.DmitrovkaColorName), "Дмитровка");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.PhilimonkovskoyeColorName), "Филимоньковская");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.OtherColorName), "Др. площадки");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.WithoutColorName), "Без аудитории");
            AddNote(worksheet, startNoteY + i++, startNoteX + 1, WorkloadHelper.GetColorByName(settingsCategory, WorkloadHelper.TransitionColorName), "Переезд");

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

        private static string SwitchClassId(LessonModel lesson)
        {
            if (lesson.Wight > 2)
                return SwitchClassId(lesson.Time) + "-" +
                   (lesson.LessionType.Length < 3 ? lesson.LessionType
                    : lesson.LessionType.ToUpper()[0..3]);

            return SwitchClassId(lesson.Time);
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

