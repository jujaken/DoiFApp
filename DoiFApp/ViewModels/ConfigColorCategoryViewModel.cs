using CommunityToolkit.Mvvm.ComponentModel;
using DoiFApp.Config;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels
{
    public partial class ConfigColorCategoryViewModel(ConfigColorCategory configColor) : ObservableObject
    {
        [ObservableProperty]
        private string tittle = configColor.Tittle;

        [ObservableProperty]
        private ObservableCollection<ConfigColorViewModel> colors = [.. configColor.Colors.Select(c => new ConfigColorViewModel(c))];
    }
}
