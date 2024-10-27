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
        private ObservableCollection<NonEducationWorkViewModel> nonEducationWorks =
        [
            new (new() { Text = "Работа 1" }) { IsFirstSemester = true, IsSecondSemester = false },
            new (new() { Text = "Работа 2" }) { IsFirstSemester = false, IsSecondSemester = true },
            new (new() { Text = "Работа 3" }) { IsFirstSemester = true, IsSecondSemester = true },
        ];

        [ObservableProperty]
        private bool isLoad;

        public event Action? OnCancel;

        [RelayCommand]
        public void Cancel()
        {
            OnCancel?.Invoke();
        }

        public event Func<NonEducationWorkViewModel[], Task>? OnOk;

        [RelayCommand(CanExecute = nameof(IsLoad))]
        public async Task Ok()
        {
            if (OnOk != null)
                await OnOk.Invoke([.. NonEducationWorks]);
        }
    }
}
