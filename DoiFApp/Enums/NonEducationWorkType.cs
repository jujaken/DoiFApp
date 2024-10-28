using DoiFApp.Utils.Attributes;

namespace DoiFApp.Enums
{
    public enum NonEducationWorkType
    {
        [ViewName("Не установлено...")]
        [IPTableId(-1, -1)]
        None,

        [ViewName("Методическая работа")]
        [IPTableId(5, 6)]
        Methodic,

        [ViewName("Научная работа")]
        [IPTableId(7, 8)]
        Scientic,

        [ViewName("Морально-психологическая работа")]
        [IPTableId(9, 10)]
        Moral,

        [ViewName("Работа с иностранными слушателями")]
        [IPTableId(11, 12)]
        Foreignic,

        [ViewName("Иная работа")]
        [IPTableId(13, 14)]
        Other,
    }
}
