using DoiFApp.Data;
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using OfficeOpenXml;

namespace DoiFApp.Services.Excel
{
    public class ExcelReader(AppDbContext context, IRepo<LessonModel> lessonRepo) : IDataReader
    {
        private readonly AppDbContext context = context;
        private readonly IRepo<LessonModel> lessonRepo = lessonRepo;

        public async Task ReadToData(string path)
        {
            context.Recreate();

            using var package = new ExcelPackage(path);

            var data = package.Workbook.Worksheets[1];

            for (int i = 5; i < data.Dimension.End.Row; i++)
            {
                if (data.Cells[i, 1].GetCellValue<string>() == null)
                    break;

                var inputData = new LessonModel
                {
                    Date = LessonModel.GetDateOnly(data.Cells[i, 1].GetCellValue<string>()),
                    Time = data.Cells[i, 3].GetCellValue<string>(),

                    Discipline = data.Cells[i, 5].GetCellValue<string>(),
                    LessionType = data.Cells[i, 6].GetCellValue<string>(),
                    Topic = data.Cells[i, 7].GetCellValue<string?>(),

                    Groups = data.Cells[i, 4].GetCellValue<string>()
                        .Split(',').Select(s => s.Trim()).ToList(),
                    Teachers = data.Cells[i, 8].GetCellValue<string>()
                        .Split('\n').Select(s => s.Trim()).ToList(),

                    Auditoriums = (data.Cells[i, 9].GetCellValue<string?>() ?? "без аудитории")
                        .Split(',').Select(s => s.Trim()).ToList()
                };

                if (inputData.LessionType.Contains("зач", StringComparison.CurrentCultureIgnoreCase)
                    || inputData.LessionType.Contains("экз", StringComparison.CurrentCultureIgnoreCase))
                {
                    var lesson = (await lessonRepo.GetWhere(lesson => lesson.Date == inputData.Date
                        && lesson.Topic == inputData.Topic
                        && lesson.Discipline == inputData.Discipline)).FirstOrDefault();

                    if (lesson != null)
                    {
                        lesson.Wight += 2;
                        await lessonRepo.Update(lesson);
                        continue;
                    }
                }

                await lessonRepo.Create(inputData);
            }
        }
    }
}
