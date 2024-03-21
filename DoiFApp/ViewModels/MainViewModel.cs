using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ToolViewModel> tools = [];

        [ObservableProperty]
        private ObservableCollection<NotifyViewModel> notifies = [];

        public MainViewModel()
        {
            tools.Add(new ToolViewModel()
            {
                Title = "Загрузи эксель",
                Description = "Загружает таблицу excel и формирует необходимые данные для работы приложения",
                Command = LoadExcelCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Выгрузи во временный файл",
                Description = "Выгружает таблицу excel в temp таблицу",
                Command = ExtractToTempFileCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Загрузи temp-файл",
                Description = "Загружает temp-файл для для просмотра",
                Command = LoadTempFileCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Сформируй загруженность",
                Description = "Формирует таблицу загруженности",
                Command = ExctractWorkloadTableCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Сформируй отчёт",
                Description = "Формирует таблицу-отчёт",
                Command = ExctractReportTableCommand
            });

        }

        [RelayCommand]
        public async Task LoadExcel()
        {
            var notify = new NotifyViewModel()
            {
                Title = "Excel таблица была загружена!",
                Description = "Теперь, выгрузите её в temp файл",
            };
            notify.OnRemove += () => Notifies.Remove(notify);
            Notifies.Add(notify);
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
