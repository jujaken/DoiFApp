using DoiFApp.Enums;

namespace DoiFApp.Data.Models
{
    public class NonEducationWork : Model
    {
        public required string Text { get; set; }
        public NonEducationWorkType Type { get; set; }
        public SemesterType Semester { get; set; }
    }
}
