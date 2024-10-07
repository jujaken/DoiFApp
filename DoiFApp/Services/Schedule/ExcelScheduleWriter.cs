using DoiFApp.Services.Data;
using OfficeOpenXml;
using System.IO;
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
            var data = package.Workbook.Worksheets["Расписание"];

            // header
            data.Cells[1, 1].Value = "Месяц";
            data.Cells[1, 2].Value = "Дисциплина";
            data.Cells[1, 3].Value = "Вид занятия";
            data.Cells[1, 4].Value = "Группы";
            data.Cells[1, 5].Value = "Преподаватель";
            data.Cells[1, 6].Value = "Часы";

            int i = 2;
            foreach(var lesson in modelData.Lessons!)
            {
                lesson.Teachers.ForEach(teacher =>
                {
                    data.Cells[i, 1].Value = lesson.Month;
                    data.Cells[i, 2].Value = lesson.Discipline;
                    data.Cells[i, 3].Value = lesson.LessionType;
                    data.Cells[i, 4].Value = lesson.GroupsText;
                    data.Cells[i, 5].Value = teacher;
                    data.Cells[i, 6].Value = lesson.Wight;
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
    }
}
