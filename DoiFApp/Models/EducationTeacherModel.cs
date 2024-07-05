namespace DoiFApp.Models
{
    public class EducationTeacherModel(string name)
    {
        public string Name { get; set; } = name;
        public List<EducationWorkModel> Works { get; set; } = [];
    }
}
