using DoiFApp.Utils;
using System.Text;

namespace DoiFApp.Data.Models
{
    public class LessonModel : Model
    {
        public DateOnly Date { get; set; }
        public string Month => DateUtil.SwitchMonth(Date.Month);
        public string DayOfWeek => DateUtil.SwitchDayOfWeek(Date.DayOfWeek);

        public string Time { get; set; } = string.Empty;

        public string Discipline { get; set; } = string.Empty;
        public string LessionType { get; set; } = string.Empty;
        public string? Topic { get; set; }

        public List<string> Teachers { get; set; } = [];
        public string TeachersText => GetListStr(Teachers, "\n");

        public List<string> Groups { get; set; } = [];
        public string GroupsText => GetListStr(Groups, ", ");

        public List<string> Auditoriums { get; set; } = [];

        public double Wight { get; set; } = 2;

        public static DateOnly GetDateOnly(string str)
        {
            var s = str.Split('.');
            return new DateOnly(Convert.ToInt32(s[2]), Convert.ToInt32(s[1]), Convert.ToInt32(s[0]));
        }

        private static string GetListStr(List<string> items, string v)
        {
            var strBuilder = new StringBuilder();

            for (int i = 0; i < items.Count - 1; i++)
                strBuilder.Append(items[i] + v);
            strBuilder.Append(items[^1]);

            return strBuilder.ToString();
        }
    }
}
