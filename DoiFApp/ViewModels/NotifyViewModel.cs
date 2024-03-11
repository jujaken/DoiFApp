using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Media;

namespace DoiFApp.ViewModels
{
    public partial class NotifyViewModel : ObservableObject
    {
        [ObservableProperty]
        private Color color;

        [ObservableProperty]
        private string? title;

        [ObservableProperty]
        private string? description;
    }
}
