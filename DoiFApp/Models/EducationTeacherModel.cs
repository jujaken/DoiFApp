namespace DoiFApp.Models
{
    public class EducationTeacherModel(string name)
    {
        public string Name { get; set; } = name;
        public Dictionary<string, double> Works1 { get; set; } = []; // первое 
        public Dictionary<string, double> Works2 { get; set; } = []; // второе полугодие
    }
}
