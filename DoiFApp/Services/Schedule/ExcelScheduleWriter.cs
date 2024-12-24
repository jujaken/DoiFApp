using DoiFApp.Services.Data;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using System.Text;
using ExcelIn = Microsoft.Office.Interop.Excel;

namespace DoiFApp.Services.Schedule
{
    public class ExcelScheduleWriter : IDataWriter<ScheduleData>
    {
        private readonly static string reportSimplePath = "Resources/reportsimple.xlsx";

        public Task<bool> Write(ScheduleData modelData, string path)
        {
            if (!modelData.IsHolistic) return Task.FromResult(false);

            File.Copy(reportSimplePath, path, true);

            using var package = new ExcelPackage(path);
            if (!package.Workbook.Worksheets.Where(w => w.Name == "Расписание").Any())
                package.Workbook.Worksheets.Add("Расписание");
            var data = package.Workbook.Worksheets["Расписание"];

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
            data.Cells[1, 10].Value = "Месяц";

            int i = 2;
            foreach (var lesson in modelData.Lessons!)
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
                    data.Cells[i, 10].Value = lesson.Month;
                    i++;
                });
            };

            package.Save();

            var excelApp = new ExcelIn.Application
            {
                Visible = false,
            };

            var workbook = excelApp.Workbooks.Open(path);
            workbook.RefreshAll();
            workbook.Save();
            workbook.Close();
            excelApp.Quit();

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
