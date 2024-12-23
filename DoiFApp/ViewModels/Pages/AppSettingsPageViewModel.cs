using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Config;
using DoiFApp.Services;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels.Pages
{
    public partial class AppSettingsPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ConfigColorCategoryViewModel> configColorCategories = [];

        private const string filePath = "doif-colors.json";

        [RelayCommand]
        public async Task Load()
        {
            var appConfigService = Ioc.Default.GetRequiredService<IAppConfigService>();
            var config = await appConfigService.Get(filePath)
                ?? await appConfigService.SetDefault(filePath);

            foreach (var category in config.ConfigColorCategories)
                ConfigColorCategories.Add(new(category));
        }
    }
}
