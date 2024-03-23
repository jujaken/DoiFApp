using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Media;

namespace DoiFApp.ViewModels
{
    public partial class NotifyViewModel : ObservableObject
    {
        [ObservableProperty]
        private Color color = new() { A = 255, R = 155, G = 24, B = 42 };

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
