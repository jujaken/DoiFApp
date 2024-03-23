using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Data;
using OfficeOpenXml;
using System.IO;
using System.Text;

namespace DoiFApp.Services.Excel
{
    public class ExcelTempFileWorker(AppDbContext context, IRepo<LessonModel> lessonRepo) : ITempFileWorker
    {
        private readonly AppDbContext context = context;
        private readonly IRepo<LessonModel> lessonRepo = lessonRepo;

        public async Task WriteFile(string path)
        {
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

            (await lessonRepo.GetAll()).ForEach(lession =>
            {
                lession.Teachers.ForEach(teacher =>
                {
                    data.Cells[i, 1].Value = lession.Date;
                    data.Cells[i, 2].Value = lession.Time;
                    data.Cells[i, 3].Value = lession.Discipline;
                    data.Cells[i, 4].Value = lession.LessionType;
                    data.Cells[i, 5].Value = lession.Topic;
                    data.Cells[i, 6].Value = GetListStr(lession.Groups, ',');
                    data.Cells[i, 8].Value = GetListStr(lession.Auditoriums, ',');
                    data.Cells[i, 7].Value = teacher;
                    data.Cells[i, 9].Value = lession.Wight;
                    i++;
                });
            });

            package.Save();
        }

        public async Task ReadFile(string path)
        {
            context.Recreate();

            using var package = new ExcelPackage(path);
            var data = package.Workbook.Worksheets[0];

            for (int i = 2; i < data.Dimension.End.Row; i++)
            {
                var inputData = new LessonModel
                {
                    Date = GetDateOnly(data.Cells[i, 1].GetCellValue<string>()),
                    Time = data.Cells[i, 2].GetCellValue<string>(),

                    Discipline = data.Cells[i, 3].GetCellValue<string>(),
                    LessionType = data.Cells[i, 4].GetCellValue<string>(),
                    Topic = data.Cells[i, 5].GetCellValue<string?>(),

                    Groups = data.Cells[i, 6].GetCellValue<string>().Split(',').Select(s => s.Trim()).ToList(),
                    Teachers = data.Cells[i, 7].GetCellValue<string>().Split('\n').Select(s => s.Trim()).ToList(),

                    Auditoriums = data.Cells[i, 8].GetCellValue<string>().Split(',').Select(s => s.Trim()).ToList(),

                    Wight = data.Cells[i, 9].GetCellValue<int>(),
                };

                var existingLesson = await lessonRepo.GetWhere(l => l.Date == inputData.Date
                      && l.Time == inputData.Time
                      && l.Discipline == inputData.Discipline
                      && l.Groups == inputData.Groups);

                if (existingLesson.Any())
                {
                    var r = existingLesson.First();
                    r.Teachers.Add(inputData.Teachers.First());
                    await lessonRepo.Update(r);
                }
                else
                {
                    await lessonRepo.Create(inputData);
                }
            }
        }

        private DateOnly GetDateOnly(string str)
        {
            var s = str.Split('.');
            return new DateOnly(Convert.ToInt32(s[2]), Convert.ToInt32(s[1]), Convert.ToInt32(s[0]));
        }

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
