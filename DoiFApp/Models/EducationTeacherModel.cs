namespace DoiFApp.Models
{
    public class EducationTeacherModel(string name)
    {
        public string Name { get; set; } = name;
        public List<EducationWorkModel> Works1 { get; set; } = []; // первое 
        public List<EducationWorkModel> Works2 { get; set; } = []; // второе полугодие
    }
}
