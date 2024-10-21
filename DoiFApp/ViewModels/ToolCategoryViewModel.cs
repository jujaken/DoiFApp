using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels
{
    public partial class ToolCategoryViewModel() : ObservableObject
    {
        [ObservableProperty]
        private string name = "Название категории";

        [ObservableProperty]
        private ObservableCollection<ToolCategoryViewModel> categories = [];

        [ObservableProperty]
        private ObservableCollection<ToolViewModel> tools = [];

        public ToolCategoryViewModel(string name, IEnumerable<ToolCategoryViewModel>  categories) : this(name)
        {
            foreach (var category in categories)
                Categories.Add(category);
        }

        public ToolCategoryViewModel(string name, IEnumerable<ToolViewModel> tools) : this(name)
        {
            foreach (var tool in tools)
                Tools.Add(tool);
        }
        public ToolCategoryViewModel(string name) : this()
        {
            this.name = name;
        }

        public ToolCategoryViewModel(string name, params ToolCategoryViewModel[] vms) : this(name, vms.AsEnumerable()) { }
        public ToolCategoryViewModel(string name, params ToolViewModel[] vms) : this(name, vms.AsEnumerable()) { }
    }
}
