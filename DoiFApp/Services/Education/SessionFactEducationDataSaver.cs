using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;

namespace DoiFApp.Services.Education
{
    public class SessionFactEducationDataSaver(IRepo<EducationTeacherModel> repo) : AbstractSessionEducationDataSaver<FactEducationData>(repo)
    {
    }
}
