﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DoiFApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        public NotifyViewModel? currentNotify;

        [RelayCommand]
        public async Task LoadExcel()
        {
            
        }

        [RelayCommand]
        public async Task ExtractToTempFile()
        {

        }

        [RelayCommand]
        public async Task LoadTempFile()
        {

        }

        [RelayCommand]
        public async Task ExctractWorkloadTable()
        {

        }

        [RelayCommand]
        public async Task ExctractReportTable()
        {

        }
    }
}