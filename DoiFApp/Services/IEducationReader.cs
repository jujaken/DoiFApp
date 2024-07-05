using DoiFApp.Models;

namespace DoiFApp.Services
{
    public interface IEducationReader
    {
        Task<List<EducationTeacherModel>> ReadFromFile(string fileName);
    }
}
