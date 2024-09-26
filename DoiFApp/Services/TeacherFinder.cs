using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using Microsoft.EntityFrameworkCore;

namespace DoiFApp.Services
{
    public class TeacherFinder(IRepo<EducationTeacherModel> teacherRepo) : ITeacherFinder
    {
        private readonly IRepo<EducationTeacherModel> teacherRepo = teacherRepo;

        public async Task<EducationTeacherModel?> FindByPart(string part)
        {
            var teacher = (await teacherRepo.GetWhere(t => t.Name.Contains(part, StringComparison.CurrentCultureIgnoreCase))).FirstOrDefault();
            if (teacher == null) return null;

            return await teacherRepo.Set
                    .Include(at => at.Works1)
                        .ThenInclude(w => w.TypesAndHours)
                    .Include(at => at.Works2)
                        .ThenInclude(w => w.TypesAndHours)
                    .Include(at => at.ReallyWorks1)
                        .ThenInclude(w => w.TypesAndHours)
                    .Include(at => at.ReallyWorks2)
                        .ThenInclude(w => w.TypesAndHours)
                .Where(t => teacher.Id == t.Id).FirstOrDefaultAsync();
        }
    }
}
