using System.Drawing;

namespace DoiFApp.Utils
{
    public static class WorkloadHelper
    {
        public static Color SaturdayColor => Color.FromArgb(255, 255, 192, 203); // low red
        public static Color SundayColor => Color.FromArgb(255, 255, 0, 0); // red

        public static Color KoptevoColor => Color.FromArgb(255, 199, 199, 199); // grey
        public static Color VolginoColor => Color.FromArgb(255, 255, 255, 230); // yellow
        public static Color OtherColor => Color.FromArgb(255, 255, 230, 200); // orange
        public static Color TransitionColor => Color.FromArgb(255, 100, 245, 100); // green
        public static Color WithoutColor => Color.FromArgb(255, 0, 200, 200); // blue

        public static Color SwitchColorByAuditorium(string id)
            => id switch
            {
                "1" => VolginoColor,
                "2" => VolginoColor,
                "3" => VolginoColor,
                "5" => KoptevoColor,
                _ => OtherColor,
            };

        public static Color SelectCellColor(List<string> auditoriums)
        {
            var auds = auditoriums.Where(a => a.Contains("к/"));

            if (!auds.Any())
                return WithoutColor;

            var suites = auds.Select(a => a.Split("к/")[0]).Distinct();
            if (suites.Count() > 1)
                return TransitionColor;

            return SwitchColorByAuditorium(suites.First());
        }
    }
}
