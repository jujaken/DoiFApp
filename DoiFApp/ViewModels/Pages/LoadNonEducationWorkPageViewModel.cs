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
        public string Tittle => WorkType.GetViewName();

        [ObservableProperty]
        private ObservableCollection<NonEducationWorkViewModel> nonEducationWorks =
        [
            new (new() { Text = "Работа 1" }) { IsFirstSemester = true, IsSecondSemester = false },
            new (new() { Text = "Работа 2" }) { IsFirstSemester = false, IsSecondSemester = true },
            new (new() { Text = "Работа 3" }) { IsFirstSemester = true, IsSecondSemester = true },
        ];

        public const string SelectAll = "Выбр.";
        public const string UnselectAll = "Убр.";

        [ObservableProperty]
        private string toggleSelectionsText = SelectAll;

        [RelayCommand]
        public void ToggleSelections()
            => ToggleProperty((work, value) => work.IsSelected = value,
                () => ToggleSelectionsText,
                (value) => ToggleSelectionsText = value);

        [ObservableProperty]
        private string toggleFirstsText = SelectAll;

        [RelayCommand]
        public void ToggleFirsts()
            => ToggleProperty((work, value) => work.IsFirstSemester = value,
                () => ToggleFirstsText,
                (value) => ToggleFirstsText = value);

        [ObservableProperty]
        private string toggleSecondsText = SelectAll;

        [RelayCommand]
        public void ToggleSeconds()
            => ToggleProperty((work, value) => work.IsSecondSemester = value,
                () => ToggleSecondsText,
                (value) => ToggleSecondsText = value);

        private void ToggleProperty(Action<NonEducationWorkViewModel, bool> foreachAction, Func<string> getter, Action<string> setter)
        {
            if (getter() == SelectAll)
            {
                foreach (var work in NonEducationWorks) foreachAction(work, true);
                setter(UnselectAll);
                return;
            }
            foreach (var work in NonEducationWorks) foreachAction(work, false);
            setter(SelectAll);
        }

        public event Action? OnCancel;

        [RelayCommand]
        public void Cancel()
        {
            OnCancel?.Invoke();
        }

        public event Func<NonEducationWorkViewModel[], Task>? OnOk;

        [RelayCommand]
        public async Task Ok()
        {
            if (OnOk != null)
                await OnOk.Invoke([.. NonEducationWorks]);
        }
    }
}
