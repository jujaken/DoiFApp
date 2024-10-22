namespace DoiFApp.Data.Models
{
    public class NonEducationWork : Model
    {
        public required string Text { get; set; }
        public NonEducationWorkType Type { get; set; }
        public NonEducationWorkSemesterType Semester { get; set; }

    }

    public enum NonEducationWorkSemesterType
    {
         None,
         First,
         Second,
    }

    public enum NonEducationWorkType
    {
        None,
        Methodic,
        Scientic,
        Educational,
        Foreignic,
        Other,
    }
}
