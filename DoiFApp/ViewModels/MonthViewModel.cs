using CommunityToolkit.Mvvm.ComponentModel;

namespace DoiFApp.ViewModels
{
    public partial class MonthViewModel : ObservableObject
    {
        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string name = "чабль";

        [ObservableProperty]
        private bool isSelected;
    }
}
