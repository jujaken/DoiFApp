namespace DoiFApp.Data.Models
{
    public class EducationWorkModel(string name) : Model
    {
        public string Name { get; set; } = name;
        public List<EducationTypeAndHourModel> TypesAndHours { get; set; } = [];
    }
}
