using System.Windows.Media;

namespace DoiFApp.Utils
{
    public static class ColorUtils
    {
        public static string ColorToHex(Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        public static Color HexToColor(string hex)
        {
            if (hex.StartsWith('#'))
                hex = hex[1..];

            //if (hex.Length == 8)
            //{
            //    byte a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            //    byte r = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            //    byte g = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //    byte b = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            //    return Color.FromArgb(a, r, g, b);
            //}
            //else 
            if (hex.Length == 6)
            {
                byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                return Color.FromArgb(255, r, g, b);
            }
            else
            {
                throw new ArgumentException("Invalid HEX color format. Use #AARRGGBB or #RRGGBB.");
            }
        }

    }
}
