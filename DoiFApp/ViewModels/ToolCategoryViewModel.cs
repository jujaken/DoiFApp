using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels
{
    public partial class ToolCategoryViewModel() : ObservableObject
    {
        [ObservableProperty]
        private string name = "Название категории";
        [ObservableProperty]
        private ObservableCollection<ToolViewModel> tools = [];

        public ToolCategoryViewModel(string name) : this()
        {
            this.name = name;
        }

        public ToolCategoryViewModel(string name, IEnumerable<ToolViewModel> vms) : this(name)
        {
            foreach (var vm in vms)
                tools.Add(vm);
        }

        public ToolCategoryViewModel(string name, params ToolViewModel[] vms) : this(name, vms.AsEnumerable()) { }
    }
}
