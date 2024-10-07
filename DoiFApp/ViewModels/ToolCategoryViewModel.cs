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

        public ToolCategoryViewModel(IEnumerable<ToolViewModel> vms) : this()
        {
            foreach (var vm in vms)
                tools.Add(vm);
        }

        public ToolCategoryViewModel(params ToolViewModel[] vms) : this(vms.AsEnumerable()) { }
    }
}
