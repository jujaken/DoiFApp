using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ToolViewModel> commands = [];


        public MainViewModel()
        {
            commands.Add(new ToolViewModel()
            {
                Title = "Загрузи эксель",
                Description = "Загружает таблицу excel и формирует необходимые данные для работы приложения",
                Command = LoadExcelCommand
            });

            commands.Add(new ToolViewModel()
            {
                Title = "Выгрузи во временный файл",
                Description = "Выгружает таблицу excel в temp таблицу",
                Command = ExtractToTempFileCommand
            });

            commands.Add(new ToolViewModel()
            {
                Title = "Загрузи temp-файл",
                Description = "Загружает temp-файл для для просмотра",
                Command = LoadTempFileCommand
            });

            commands.Add(new ToolViewModel()
            {
                Title = "Сформируй загруженность",
                Description = "Формирует таблицу загруженности",
                Command = ExctractWorkloadTableCommand
            });

            commands.Add(new ToolViewModel()
            {
                Title = "Сформируй отчёт",
                Description = "Формирует таблицу-отчёт",
                Command = ExctractReportTableCommand
            });
        }

        [ObservableProperty]
        private ObservableCollection<NotifyViewModel> notifies = [];

        [RelayCommand]
        public async Task LoadExcel()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public async Task ExtractToTempFile()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public async Task LoadTempFile()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public async Task ExctractWorkloadTable()
        {
            throw new NotImplementedException();
        }

        [RelayCommand]
        public async Task ExctractReportTable()
        {
            throw new NotImplementedException();
        }
    }
}
