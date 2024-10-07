using CommunityToolkit.Mvvm.ComponentModel;
using DoiFApp.Utils;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ToolCategoryViewModel> toolsCategories = [];

        [ObservableProperty]
        private ObservableCollection<NotifyViewModel> notifies = [];

        [ObservableProperty]
        private object? curPage;

        public MainViewModel()
        {
            ToolsHelper.AddDefaults(toolsCategories);
        }
    }
}
