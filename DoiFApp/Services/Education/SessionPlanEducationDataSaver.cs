using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;

namespace DoiFApp.Services.Education
{
    public class SessionPlanEducationDataSaver(IRepo<EducationTeacherModel> repo) : AbstractSessionEducationDataSaver<PlanEducationData>(repo)
    {
    }
}
