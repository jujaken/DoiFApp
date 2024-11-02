using CommunityToolkit.Mvvm.ComponentModel;
using DoiFApp.Utils;

namespace DoiFApp.ViewModels
{
    public partial class LessonTypeTranslateViewModel : ObservableObject
    {
        [ObservableProperty]
        private string currentName = "Лекция";

        [ObservableProperty]
        private string newName = "СуперЛекция";

        [ObservableProperty]
        private string? selectedConvertion;

        public static string[] Convertions => TableDataUtil.GetHeaders(TableDataUtil.InputCommonTableHeaders).ToArray();
    }
}
