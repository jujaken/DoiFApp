using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using OfficeOpenXml;
using System.IO;
using System.Text;
using ExcelIn = Microsoft.Office.Interop.Excel;

namespace DoiFApp.Services.Excel
{
    public class ExcelReportWriter(IRepo<LessonModel> lessonRepo) : IReportWriter
    {
        private readonly IRepo<LessonModel> lessonRepo = lessonRepo;

        private static string reportSimplePath = "Resources/reportsimple.xlsx";

        public async Task Write(string path)
        {
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
            (await lessonRepo.GetAll()).ForEach(lession =>
            {
                lession.Teachers.ForEach(teacher =>
                {
                    data.Cells[i, 1].Value = SwitchMonth(lession.Date.Month);
                    data.Cells[i, 2].Value = lession.Discipline;
                    data.Cells[i, 3].Value = lession.LessionType;
                    data.Cells[i, 4].Value = GetListStr(lession.Groups, ',');
                    data.Cells[i, 5].Value = teacher;
                    data.Cells[i, 6].Value = lession.Wight;
                    i++;
                });
            });

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
        }

        private string SwitchMonth(int num)
            => num switch
            {
                1 => "январь",
                2 => "февраль",
                3 => "март",
                4 => "апрель",
                5 => "май",
                6 => "июнь",
                7 => "июль",
                8 => "август",
                9 => "сентябрь",
                10 => "октябрь",
                11 => "ноябрь",
                12 => "декабрь",
                _ => throw new Exception()
            };

        private string GetListStr(List<string> items, char v)
        {
            var strBuilder = new StringBuilder();

            for (int i = 0; i < items.Count - 1; i++)
                strBuilder.Append(items[i] + v);
            strBuilder.Append(items[^1]);

            return strBuilder.ToString();
        }
    }
}
