using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Enums;
using DoiFApp.Utils.Extensions;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels.Pages
{
    public partial class LoadNonEducationWorkPageViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Tittle))]
        private NonEducationWorkType workType;
        public string Tittle => WorkType.GetDescription();

        [ObservableProperty]
        private ObservableCollection<NonEducationWorkViewModel> nonEducationWorks = [];

        [ObservableProperty]
        private bool isLoad;

        public event Action? OnLoad;

        [RelayCommand]
        public void Load()
        {
            OnLoad?.Invoke();
        }

        public event Action? OnExtract;

        [RelayCommand]
        public void Extract()
        {
            OnExtract?.Invoke();
        }

        public event Action? OnCancel;

        [RelayCommand]
        public void Cancel()
        {
            OnCancel?.Invoke();
        }

        public event Func<NonEducationWorkType, Task>? OnOk;

        [RelayCommand(CanExecute = nameof(IsLoad))]
        public async Task Ok()
        {
            if (OnOk != null)
                await OnOk.Invoke(WorkType);
        }
    }
}
