using DoiFApp.Data.Models;

namespace DoiFApp.Services
{
    public interface ITeacherFinder
    {
        Task<EducationTeacherModel?> FindByPart(string part);
    }
}
