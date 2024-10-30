using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using Microsoft.EntityFrameworkCore;

namespace DoiFApp.Services
{
    public class TeacherFinder(IRepo<EducationTeacherModel> teacherRepo) : ITeacherFinder
    {
        private readonly IRepo<EducationTeacherModel> teacherRepo = teacherRepo;

        public async Task<List<EducationTeacherModel>?> FindByPart(string? part, bool needInclude = false)
        {
            if (part == null) return (await teacherRepo.GetAll()).ToList();

            var teacher = (await teacherRepo.GetWhere(t => t.Name.Contains(part, StringComparison.CurrentCultureIgnoreCase))).FirstOrDefault();
            if (teacher == null) return null;

            if (needInclude)
                return await teacherRepo.Set
                        .Include(at => at.Works)
                            .ThenInclude(w => w.TypesAndHours)
                    .Where(t => teacher.Id == t.Id).ToListAsync();

            return [.. (await teacherRepo.GetWhere(t => teacher.Id == t.Id))];
        }
    }
}
