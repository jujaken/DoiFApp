using System.ComponentModel.DataAnnotations.Schema;

namespace DoiFApp.Data.Models
{
    public class EducationTeacherModel(string name) : Model
    {
        public string Name { get; set; } = name;
        public List<EducationWorkModel> Works { get; set; } = [];

        [NotMapped]
        public List<EducationWorkModel> PlanWorks1
            => Works.Where(w => w.WorkCategory == WorkCategory.PlanFirstSemester).ToList();
        
        [NotMapped]
        public List<EducationWorkModel> PlanWorks2
            => Works.Where(w => w.WorkCategory == WorkCategory.PlanSecondSemester).ToList();

        [NotMapped]
        public List<EducationWorkModel> FactWorks1
            => Works.Where(w => w.WorkCategory == WorkCategory.FactFirstSemester).ToList();

        [NotMapped]
        public List<EducationWorkModel> FactWorks2
            => Works.Where(w => w.WorkCategory == WorkCategory.FactSecondSemester).ToList();
    }
}
