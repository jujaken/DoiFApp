using DoiFApp.Models;

namespace DoiFApp.Services
{
    public interface IIndividualPlanWriter
    {
        Task MakePlans(List<EducationTeacherModel> data, string path);
    }
}
