using DoiFApp.Services.Data;
using OfficeOpenXml;
using System.IO;
using System.Text;

namespace DoiFApp.Services.TempSchedule
{
    public class ExcelTempScheduleWriter : IDataWriter<TempScheduleData>
    {
        public Task<bool> Write(TempScheduleData scheduleData, string path)
        {
            if (!scheduleData.IsHolistic) return Task.FromResult(false);

            if (File.Exists(path))
                File.Delete(path);

            using var package = new ExcelPackage(path);
            var data = package.Workbook.Worksheets.Add("Расписание");

            // header
            data.Cells[1, 1].Value = "Дата";
            data.Cells[1, 2].Value = "Время";
            data.Cells[1, 3].Value = "Дисциплина";
            data.Cells[1, 4].Value = "Вид занятия";
            data.Cells[1, 5].Value = "Тема";
            data.Cells[1, 6].Value = "Группы";
            data.Cells[1, 7].Value = "Преподаватель";
            data.Cells[1, 8].Value = "Аудитории";
            data.Cells[1, 9].Value = "Часы";

            int i = 2;
            foreach (var lesson in scheduleData.Lessons!)
            {
                lesson.Teachers.ForEach(teacher =>
                {
                    data.Cells[i, 1].Value = lesson.Date.ToString();
                    data.Cells[i, 2].Value = lesson.Time;
                    data.Cells[i, 3].Value = lesson.Discipline;
                    data.Cells[i, 4].Value = lesson.LessionType;
                    data.Cells[i, 5].Value = lesson.Topic;
                    data.Cells[i, 6].Value = GetListStr(lesson.Groups, ',');
                    data.Cells[i, 8].Value = GetListStr(lesson.Auditoriums, ',');
                    data.Cells[i, 7].Value = teacher;
                    data.Cells[i, 9].Value = lesson.Wight;
                    i++;
                });
            };

            package.Save();
            return Task.FromResult(true);
        }

        private static string GetListStr(List<string> items, char v)
        {
            var strBuilder = new StringBuilder();

            for (int i = 0; i < items.Count - 1; i++)
                strBuilder.Append(items[i] + v);
            strBuilder.Append(items[^1]);

            return strBuilder.ToString();
        }
    }
}
