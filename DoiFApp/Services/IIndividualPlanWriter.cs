using DoiFApp.Data.Models;

namespace DoiFApp.Services
{
    public interface IIndividualPlanWriter
    {
        Task FillPlan(EducationTeacherModel teacher, string path);
    }
}
