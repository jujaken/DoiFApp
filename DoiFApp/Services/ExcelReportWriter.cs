using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using OfficeOpenXml;
using System.IO;

namespace DoiFApp.Services
{
    public class ExcelReportWriter(IRepo<LessonModel> lessonRepo) : IReportWriter
    {
        private readonly IRepo<LessonModel> lessonRepo = lessonRepo;

        public async Task Write(string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            using var package = new ExcelPackage(path);
            var data = package.Workbook.Worksheets.Add("Таблица по месяцам");

            (await lessonRepo.GetAll()).ForEach(l =>
            {

            });
        }
    }
}
