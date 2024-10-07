using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services.Data;

namespace DoiFApp.Services.Education
{
    public class SessionEducationSaver(IRepo<EducationTeacherModel> repo) : AbstractSessionDataSaver<EducationTeacherModel, EducationData>(repo)
    {
    }
}
