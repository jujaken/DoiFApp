using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services.Schedule;

namespace DoiFApp.Services.TempSchedule
{
    internal class ExcelTempScheduleSaver(IRepo<LessonModel> repo) : SessionScheduleSaver(repo)
    {
    }
}
