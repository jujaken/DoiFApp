using DoiFApp.Models;

namespace DoIFToolApp.Models.Data
{
    public class LessonModel : Model
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string DisciplineName { get; set; } = string.Empty;
        public string LessionTypeName { get; set; } = string.Empty;

        public List<string> TeacherNames { get; set; } = [];
        public List<string> GroupNames { get; set; } = [];

        public int Wight { get; set; }
    }
}
