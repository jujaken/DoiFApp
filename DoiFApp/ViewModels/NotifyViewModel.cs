using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;
using System.Windows.Media;

namespace DoiFApp.ViewModels
{
    public partial class NotifyViewModel : ObservableObject
    {
        [ObservableProperty]
        private Brush color = new SolidColorBrush(new Color() { R = 50, G = 24, B = 42 });

        [ObservableProperty]
        private string? title = "Tittle";

        [ObservableProperty]
        private string? description = "Description...";

        public event Action<NotifyViewModel>? OnRemove;

        [RelayCommand]
        public Task Remove()
        {
            OnRemove?.Invoke(this);
            return Task.CompletedTask;
        }
    }
}
