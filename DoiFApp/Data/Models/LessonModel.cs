namespace DoiFApp.Data.Models
{
    public class LessonModel : Model
    {
        public DateOnly Date { get; set; }

        public string Time { get; set; } = string.Empty;

        public string Discipline { get; set; } = string.Empty;
        public string LessionType { get; set; } = string.Empty;
        public string? Topic { get; set; }

        public List<string> Teachers { get; set; } = [];
        public List<string> Groups { get; set; } = [];
        public List<string> Auditoriums { get; set; } = [];

        public double Wight { get; set; } = 2;
    }
}
