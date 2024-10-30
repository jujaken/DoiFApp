using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services.Data;

namespace DoiFApp.Services.Education
{
    public class SessionFactEducationSaver(IRepo<EducationTeacherModel> repo) : AbstractSessionDataSaver<EducationTeacherModel, FactEducationData>(repo)
    {
    }
}
