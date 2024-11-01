using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DoiFApp.ViewModels.Pages
{
    public partial class ExtractWorkloadPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private MonthViewModel[] months = [
            new () { Id = 9, Name = "Сентябрь", IsSelected = false },
            new () { Id = 10, Name = "Октябрь", IsSelected = false },
            new () { Id = 11, Name = "Ноябрь", IsSelected = false },
            new () { Id = 12, Name = "Декабрь", IsSelected = false },
            new () { Id = 1, Name = "Январь", IsSelected = false },
            new () { Id = 2, Name = "Февраль", IsSelected = false },
            new () { Id = 3, Name = "Март", IsSelected = false },
            new () { Id = 4, Name = "Апрель", IsSelected = false },
            new () { Id = 5, Name = "Май", IsSelected = false },
            new () { Id = 6, Name = "Июнь", IsSelected = false },
            new () { Id = 7, Name = "Июль", IsSelected = false },
            new () { Id = 8, Name = "Август", IsSelected = false },
            ];

        [RelayCommand]
        public void ClearAll()
        {
            foreach (var month in Months)
                month.IsSelected = false;
        }

        [RelayCommand]
        public void SelectAll()
        {
            foreach (var month in Months)
                month.IsSelected = true;
        }

        public event Action? OnCancel;

        [RelayCommand]
        public void Cancel()
        {
            OnCancel?.Invoke();
        }

        public event Func<int[], Task>? OnOk;

        [RelayCommand]
        public async Task Ok()
        {
            if (OnOk != null)
                await OnOk.Invoke([.. Months.Where(m => m.IsSelected).Select(m => m.Id)]);
        }
    }
}
