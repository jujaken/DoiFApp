using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;

namespace DoiFApp.Services
{
    public class TeacherFinder(IRepo<EducationTeacherModel> teacherRepo) : ITeacherFinder
    {
        private readonly IRepo<EducationTeacherModel> teacherRepo = teacherRepo;

        public async Task<EducationTeacherModel?> FindByPart(string part)
        {
            return (await teacherRepo
                    .Include(at => at.Works1)
                    .Include(at => at.Works2)
                    .Include(at => at.ReallyWorks1)
                    .Include(at => at.ReallyWorks2)
                .GetWhere(t => t.Name.Contains(part, StringComparison.CurrentCultureIgnoreCase)))
                .FirstOrDefault();
        }
    }
}
