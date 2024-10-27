using System.ComponentModel;

namespace DoiFApp.Enums
{
    public enum NonEducationWorkType
    {
        [Description("Не установлено...")]
        None,
        [Description("Методическая работа")]
        Methodic,
        [Description("Научная работа")]
        Scientic,
        [Description("Воспитательная работа")]
        Educational,
        [Description("Работа с иностранными слушателями")]
        Foreignic,
        [Description("Иная работа")]
        Other,
    }
}
