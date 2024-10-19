using CommunityToolkit.Mvvm.ComponentModel;

namespace DoiFApp.ViewModels
{
    public partial class LessonTypeTranslateViewModel : ObservableObject
    {
        [ObservableProperty]
        private string currentName = "Лекция";

        [ObservableProperty]
        private string newName = "СуперЛекция";
    }
}
