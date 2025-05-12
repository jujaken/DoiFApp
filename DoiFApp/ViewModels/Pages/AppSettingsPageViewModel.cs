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
        private ObservableCollection<ConfigColorCategoryViewModel> configColorCategories =
            [.. AppConfig.DefaultConfig.ConfigColorCategories.Select(c => new ConfigColorCategoryViewModel(c))];

        private const string filePath = "doif-colors.json";


        [RelayCommand]
        public async Task Load()
        {
            var appConfigService = Ioc.Default.GetRequiredService<IAppConfigService>();

            var config = await appConfigService.Get(filePath)
                ?? await appConfigService.SetDefault(filePath);

            ConfigColorCategories.Clear();

            foreach (var category in config.ConfigColorCategories)
                ConfigColorCategories.Add(new(category));
        }

        public Action? OnSave;

        [RelayCommand]
        public async Task Save()
        {
            var appConfigService = Ioc.Default.GetRequiredService<IAppConfigService>();

            await appConfigService.Save(new AppConfig()
            {
                ConfigColorCategories = [.. ConfigColorCategories.Select(category => {
                    return new ConfigColorCategory(){
                        Tittle = category.Tittle,
                        Colors = category.Colors.Select(color => {
                            return new ConfigColor() {
                                Key = color.Key,
                                Value = [color.Value.R, color.Value.G, color.Value.B]
                            };
                        }).ToList()
                    };
                })]
            }, filePath);
            OnSave?.Invoke();
        }
    }
}
