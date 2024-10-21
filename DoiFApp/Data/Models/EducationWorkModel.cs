namespace DoiFApp.Data.Models
{
    public class EducationWorkModel(string name) : Model
    {
        public string Name { get; set; } = name;
        public WorkCategory WorkCategory { get; set; }
        public EducationTeacherModel? Teacher { get; set; }
        public List<EducationTypeAndHourModel> TypesAndHours { get; set; } = [];
    }
    public enum WorkCategory
    {
        None,
        PlanFirstSemester,
        PlanSecondSemester,
        FactFirstSemester,
        FactSecondSemester,
    }
}
