using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace DoiFApp.ViewModels
{
    public partial class ToolViewModel : ObservableObject
    {
        public ToolViewModel Self => this;

        [ObservableProperty]
        private string title = "tool title";

        [ObservableProperty]
        private string description = "tool desc";

        public ICommand? Command { get; set; }
    }
}
