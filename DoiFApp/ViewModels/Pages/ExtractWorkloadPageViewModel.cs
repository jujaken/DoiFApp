﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DoiFApp.ViewModels.Pages
{
    public partial class ExtractWorkloadPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private MonthViewModel[] months = [
            new () { Id = 9, Name = "Сентябрь", IsSelected = true },
            new () { Id = 10, Name = "Октябрь", IsSelected = true },
            new () { Id = 11, Name = "Ноябрь", IsSelected = true },
            new () { Id = 12, Name = "Декабрь", IsSelected = true },
            new () { Id = 1, Name = "Январь", IsSelected = true },
            new () { Id = 2, Name = "Февраль", IsSelected = true },
            new () { Id = 3, Name = "Март", IsSelected = true },
            new () { Id = 4, Name = "Апрель", IsSelected = true },
            new () { Id = 5, Name = "Май", IsSelected = true },
            new () { Id = 6, Name = "Июнь", IsSelected = true },
            new () { Id = 7, Name = "Июль", IsSelected = true },
            new () { Id = 8, Name = "Август", IsSelected = true },
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

        // ниже вы видете отход от MVVM
        // произошло это потому, что по какой-то причине делегаты/ивенты не работают
        // если ты это исправишь, чел, ты лучший.

        [RelayCommand]
        public void Cancel()
        {
            var mvm = App.Current.MainWindow.DataContext as MainViewModel;
            mvm!.CurPage = null;
        }

        [RelayCommand]
        public async Task Ok()
        {
            var mvm = App.Current.MainWindow.DataContext as MainViewModel;
            await mvm!.ExtractWorkload([.. Months.Where(m => m.IsSelected).Select(m => m.Id)]);
        }
    }
}