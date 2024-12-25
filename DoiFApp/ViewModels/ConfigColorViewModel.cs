using CommunityToolkit.Mvvm.ComponentModel;
using DoiFApp.Config;
using DoiFApp.Utils;
using System.Windows.Media;

namespace DoiFApp.ViewModels
{
    public class ConfigColorViewModel : ObservableObject
    {
        public ConfigColorViewModel() : this(new ConfigColor() { Key = "Пример", Value = [255, 0, 0] })
        {
        }


        public ConfigColorViewModel(ConfigColor configColor)
        {
            ConfigColor = configColor;
            value = new() { A = 255, R = ConfigColor.Value[0], G = ConfigColor.Value[1], B = ConfigColor.Value[2] };
            hex = ColorUtils.ColorToHex(value);
        }

        public ConfigColor ConfigColor { get; private set; }

        public string Key
        {
            get => ConfigColor.Key;
            set => SetProperty(ConfigColor.Key, value, ConfigColor, (m, v) => m.Key = v);
        }

        private Color value;
        public Color Value
        {
            get => value;
            set
            {
                System.Windows.MessageBox.Show(value.ToString());

                if (this.value == value) return;

                SetProperty(ref this.value, value);
                Hex = ColorUtils.ColorToHex(value);
                SetProperty(ConfigColor.Value, [value.R, value.G, value.B], ConfigColor, (m, v) => m.Value = v);
            }
        }

        private string hex;
        public string Hex
        {
            get => hex;
            set
            {
                if (hex == value) return;
                if (hex.Length != 7) return;

                try
                {
                    var color = ColorUtils.HexToColor(value);
                    Value = color;
                    SetProperty(ref hex, value);
                }
                catch
                {

                }
            }
        }
    }
}
