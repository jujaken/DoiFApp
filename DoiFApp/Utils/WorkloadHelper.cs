using DoiFApp.Config;
using System.Drawing;

namespace DoiFApp.Utils
{
    public static class WorkloadHelper
    {
        public const string CategoryName = "Цвета для загруженности";

        public const string SaturdayColorName = "Цвет субботы";
        public const string SundayColorName = "Цвет воскресенья";

        /// <summary>
        /// 5
        /// </summary>
        public const string KoptevoColorName = "Цвет Коптево"; 
        /// <summary>
        /// 1-3
        /// </summary>
        public const string VolginoColorName = "Цвет Волгина";
        /// <summary>
        /// 6
        /// </summary>
        public const string BobruiskayaColorName = "Цвет Бобруйской";
        /// <summary>
        /// 7
        /// </summary>
        public const string GlavnayaColorName = "Цвет Главной 86";
        /// <summary>
        /// 8
        /// </summary>
        public const string OkrujnoyColorName = "Цвет Окружной";
        /// <summary>
        /// 9
        /// </summary>
        public const string KolskayaColorName = "Цвет Кольской";
        /// <summary>
        /// 10
        /// </summary>
        public const string DmitrovkaColorName = "Цвет Дмитровки";
        /// <summary>
        /// 11
        /// </summary>
        public const string PhilimonkovskoyeColorName = "Цвет Филимоньковской";
        public const string OtherColorName = "Цвет для других площадок";
        public const string TransitionColorName = "Цвет перехода";
        public const string WithoutColorName = "Цвет без аудитории";

        public static List<byte> SaturdayColorDefault => [255, 192, 203]; // low red
        public static List<byte> SundayColorDefault => [255, 0, 0]; // red

        public static List<byte> KoptevoColorDefault => [199, 199, 199]; // grey
        public static List<byte> VolginoColorDefault => [255, 255, 230]; // yellow
        public static List<byte> OtherColorDefault => [255, 230, 200]; // orange
        public static List<byte> TransitionColorDefault => [100, 245, 100]; // green
        public static List<byte> WithoutColorDefault => [0, 200, 200]; // blue

        public static Color GetColorByName(ConfigColorCategory configColor, string name)
        {
            var value = (configColor.Colors.Where(c => c.Key == name).FirstOrDefault() ?? throw new ArgumentException("Bad config or name!")).Value;
            return Color.FromArgb(value[0], value[1], value[2]);
        }

        public static Color SwitchColorByBuilding(ConfigColorCategory configColor, string building)
            => building switch
            {
                "Без корпуса" => GetColorByName(configColor, WithoutColorName),

                "1" => GetColorByName(configColor, VolginoColorName),
                "2" => GetColorByName(configColor, VolginoColorName),
                "3" => GetColorByName(configColor, VolginoColorName),
                "5" => GetColorByName(configColor, KoptevoColorName),

                "6" => GetColorByName(configColor, BobruiskayaColorName),
                "7" => GetColorByName(configColor, GlavnayaColorName),
                "8" => GetColorByName(configColor, OkrujnoyColorName),
                "9" => GetColorByName(configColor, KolskayaColorName),
                "10" => GetColorByName(configColor, DmitrovkaColorName),
                "11" => GetColorByName(configColor, PhilimonkovskoyeColorName),

                _ => GetColorByName(configColor, OtherColorName),
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
