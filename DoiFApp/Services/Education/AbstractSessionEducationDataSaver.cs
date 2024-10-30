using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DoiFApp.Services.Education
{
    public class AbstractSessionEducationDataSaver<T>(IRepo<EducationTeacherModel> repo) : AbstractSessionDataSaver<EducationTeacherModel, T>(repo) where T : AbstractEducationData
    {
        public override async Task<bool> Save(T data)
        {
            if (!data.IsHolistic) return false;

            var newTeachersNames = data.TeacherModels!.Select(t => t.Name);

            var coincidences = repo.Set.Where(t => newTeachersNames.Contains(t.Name))
                .Include(at => at.Works)
                    .ThenInclude(at => at.TypesAndHours);

            foreach (var teacher in coincidences)
                teacher.Works.AddRange(data.TeacherModels!
                    .Where(t => t.Name == teacher.Name)
                    .FirstOrDefault()!.Works);

            var coincidencesTeachersNames = coincidences.Select(t => t.Name);

            foreach (var teacher in data.TeacherModels!
                    .Where(t => !coincidencesTeachersNames.Contains(t.Name)))
                await repo.Set.AddAsync(teacher);

            await repo.Db.SaveChangesAsync();
            return true;
        }
    }
}
