using DoiFApp.Utils.Attributes;

namespace DoiFApp.Enums
{
    public enum NonEducationWorkType
    {
        [ViewName("Не установлено...")]
        [IDTableId(-1, -1)]
        None,

        [ViewName("Методическая работа")]
        [IDTableId(5, 6)]
        Methodic,

        [ViewName("Научная работа")]
        [IDTableId(7, 8)]
        Scientic,

        [ViewName("Морально-психологическая работа")]
        [IDTableId(9, 10)]
        Moral,

        [ViewName("Работа с иностранными слушателями")]
        [IDTableId(11, 12)]
        Foreignic,

        [ViewName("Иная работа")]
        [IDTableId(13, 14)]
        Other,
    }
}
