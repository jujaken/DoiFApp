namespace DoiFApp.Data.Models
{
    public class EducationTeacherModel(string name) : Model
    {
        public string Name { get; set; } = name;
        public List<EducationWorkModel> Works1 { get; set; } = []; // первое 
        public List<EducationWorkModel> Works2 { get; set; } = []; // второе полугодие
        public List<EducationWorkModel> ReallyWorks1 { get; set; } = []; // первое 
        public List<EducationWorkModel> ReallyWorks2 { get; set; } = []; // второе полугодие
    }
}
