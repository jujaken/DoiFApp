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

        public static Color SwitchColorByBuilding(string building)
            => building switch
            {
                "Без корпуса" => WithoutColor,
                "1" => VolginoColor,
                "2" => VolginoColor,
                "3" => VolginoColor,
                "5" => KoptevoColor,
                _ => OtherColor,
            };

        public static List<string> GetBuildings(List<string> auditoriums)
        {
            return auditoriums.Select(GetBuilding).ToList();
        }

        public static string GetBuilding(string auditory)
        {
            if (!auditory.Contains("к/"))
                return "Без корпуса";

            var id = auditory.Split("к/")[0];

            if (id == "2" || id == "3")
                return "1";

            return id;
        }
    }
}
