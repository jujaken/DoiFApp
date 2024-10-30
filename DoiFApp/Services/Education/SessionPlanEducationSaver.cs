using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services.Data;

namespace DoiFApp.Services.Education
{
    public class SessionPlanEducationSaver(IRepo<EducationTeacherModel> repo) : AbstractSessionDataSaver<EducationTeacherModel, PlanEducationData>(repo)
    {
    }
}
