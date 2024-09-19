using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;

namespace DoiFApp.Services
{
    public class TeacherFinder(IRepo<EducationTeacherModel> teacherRepo) : ITeacherFinder
    {
        private readonly IRepo<EducationTeacherModel> teacherRepo = teacherRepo;

        public async Task<EducationTeacherModel?> FindByPart(string part)
        {
            return (await teacherRepo.GetAll()).FirstOrDefault(t 
                => t.Name.Contains(part, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
