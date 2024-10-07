using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services.Data;

namespace DoiFApp.Services.Schedule
{
    public class SessionScheduleSaver(IRepo<LessonModel> repo) : AbstractSessionDataSaver<LessonModel, ScheduleData>(repo)
    {
        public override async Task<bool> Save(IData data)
        {
            repo.Db.RecreateLessons();
            return await base.Save(data);
        }
    }
}
