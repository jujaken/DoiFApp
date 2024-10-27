using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace DoiFApp.ViewModels
{
    public partial class ToolViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title = "tool title";

        [ObservableProperty]
        private string description = "tool desc";

        public ICommand? Command { get; set; }
        public object? Argument { get; set; }
    }
}
