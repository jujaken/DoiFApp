using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ToolViewModel> tools = [];

        [ObservableProperty]
        private ObservableCollection<NotifyViewModel> notifies = [];

        [ObservableProperty]
        private object? curPage;

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
                Title = "Загрузи temp-файл",
                Description = "Загружает temp-файл для для просмотра",
                Command = LoadTempFileCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Выгрузи во временный файл",
                Description = "Выгружает таблицу excel в temp таблицу",
                Command = ExtractToTempFileCommand
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
            var fileDialog = new OpenFileDialog
            {
                Filter = "excel file|*.xlsx"
            };

            fileDialog.ShowDialog();

            await Ioc.Default.GetRequiredService<IExcelReader>().ReadToData(fileDialog.FileName);
            await Notify("Данные загружены!", "Теперь, вы можете использывать другие команды!");

            try
            {


                var page = new DataPageViewModel();
                await page.LoadLessonData();
                CurPage = page;
            }
            catch
            {
                await Notify("Ошибка загрузки!", "Что-то пошло не так.", NotifyColorType.Error);
            }
        }

        [RelayCommand]
        public async Task LoadTempFile()
        {
            await Notify("Данные загружены!", "Теперь, вы можете использывать другие команды!");
        }

        public bool CanExtractToTempFile { get; protected set; }

        [RelayCommand(CanExecute = nameof(CanExtractToTempFile))]
        public async Task ExtractToTempFile()
        {
            await Notify("Данные выгружены!", "Посмотрите файл в директории!");
        }

        public bool CanExctractWorkloadTable { get; protected set; }

        [RelayCommand(CanExecute = nameof(CanExctractWorkloadTable))]
        public async Task ExctractWorkloadTable()
        {
            await Notify("Данные выгружены!", "Посмотрите файл в директории!");
        }

        public bool CanExctractReportTable { get; protected set; }

        [RelayCommand(CanExecute = nameof(CanExctractReportTable))]
        public async Task ExctractReportTable()
        {
            await Notify("Данные выгружены!", "Посмотрите файл в директории!");
        }

        private Task Notify(string title, string desc, NotifyColorType colorType = NotifyColorType.Info)
        {
            var notify = Ioc.Default.GetRequiredService<NotifyBuilder>()
                .WithTitle(title)
                .WithDescription(desc)
                .WithColor(colorType)
                .WithRemove(Notifies.Remove)
                .Build();

            Notifies.Add(notify);

            return Task.CompletedTask;
        }
    }
}
