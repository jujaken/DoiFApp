using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

            foreach (var repoTeacher in coincidences)
            {
                var newTeacher = data.TeacherModels!.Where(t => t.Name == repoTeacher.Name).FirstOrDefault()!;

                // PlanWorks1

                if (newTeacher.PlanWorks1.Count != 0)
                    repoTeacher.PlanWorks1.ForEach(w => repoTeacher.Works.Remove(w));

                if (repoTeacher.PlanWorks1.Count == 0)
                    repoTeacher.Works.AddRange(newTeacher.PlanWorks1);

                // PlanWorks2

                if (newTeacher.PlanWorks2.Count != 0)
                    repoTeacher.PlanWorks2.ForEach(w => repoTeacher.Works.Remove(w));

                if (repoTeacher.PlanWorks2.Count == 0)
                    repoTeacher.Works.AddRange(newTeacher.PlanWorks2);

                // FactWorks1

                if (newTeacher.FactWorks1.Count != 0)
                    repoTeacher.FactWorks1.ForEach(w => repoTeacher.Works.Remove(w));

                if (repoTeacher.FactWorks1.Count == 0)
                    repoTeacher.Works.AddRange(newTeacher.FactWorks1);

                // FactWorks2

                if (newTeacher.FactWorks2.Count != 0)
                    repoTeacher.FactWorks2.ForEach(w => repoTeacher.Works.Remove(w));

                if (repoTeacher.FactWorks2.Count == 0)
                    repoTeacher.Works.AddRange(newTeacher.FactWorks2);
            }

            var coincidencesTeachersNames = coincidences.Select(t => t.Name);

            foreach (var teacher in data.TeacherModels!
                    .Where(t => !coincidencesTeachersNames.Contains(t.Name)))
                await repo.Set.AddAsync(teacher);

            await repo.Db.SaveChangesAsync();
            return true;
        }
    }
}
