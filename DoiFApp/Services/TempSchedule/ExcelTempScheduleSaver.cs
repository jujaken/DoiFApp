using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services.Data;

namespace DoiFApp.Services.TempSchedule
{
    public class ExcelTempScheduleSaver(IRepo<LessonModel> repo) : AbstractSessionDataSaver<LessonModel, TempScheduleData>(repo)
    {
        public override async Task<bool> Save(IData data)
        {
            repo.Db.RecreateLessons();
            return await base.Save(data);
        }
    }
}
