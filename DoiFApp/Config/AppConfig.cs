using System.Drawing;

namespace DoiFApp.Config
{
    public class AppConfig
    {
        public List<ConfigColorCategory> ConfigColorCategories { get; set; } = [];

        public static AppConfig DefaultConfig => new()
        {
            ConfigColorCategories =
            [
                new ConfigColorCategory() {
                    Tittle = "Цвета в отчёте по преподавателям",
                    Colors =
                    [
                        new ConfigColor() { Key = "Цвет такой-то", Value = [255, 0, 0] },
                    ]
                },
            ],
        };
    }
}
