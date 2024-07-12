namespace DoiFApp.Models
{
    public class EducationWorkModel(string name)
    {
        public string Name { get; set; } = name;
        public Dictionary<string, double> TypesAndHours { get; set; } = [];
    }
}
