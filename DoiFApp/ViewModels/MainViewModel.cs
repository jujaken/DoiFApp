using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Forms;

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

        [ObservableProperty]

        private Task? curTask;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoadSessionCommand),
                                   nameof(LoadExcelCommand),
                                   nameof(LoadTempFileCommand))]
        private bool noTask = true;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ExtractToTempFileCommand),
                                    nameof(ExctractWorkloadTableCommand),
                                    nameof(ExctractReportTableCommand))]
        private bool canExtract;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FillIndividualPlanCommand))]
        private bool isLoadCalculation;

        [ObservableProperty]
        private string? teacherName;

        public MainViewModel()
        {
            tools.Add(new ToolViewModel()
            {
                Title = "Загрузить из сессии",
                Description = "Загружает таблицу excel и формирует необходимые данные для работы приложения",
                Command = LoadSessionCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Загрузить расписание",
                Description = "Загружает таблицу excel с расписанием и формирует необходимые данные для работы приложения",
                Command = LoadExcelCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Загрузить временный файл",
                Description = "Загружает временный файл для для просмотра",
                Command = LoadTempFileCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Указать расчёт",
                Description = "Сохраняет путь к файлу расчёта",
                Command = UploadCalculationCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Выгрузить во временный файл",
                Description = "Выгружает таблицу excel во временный файл таблицу",
                Command = ExtractToTempFileCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Выдать загруженность",
                Description = "Формирует таблицу загруженности",
                Command = ExctractWorkloadTableCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Выдать отчёт",
                Description = "Формирует таблицу-отчёт по преподавателям",
                Command = ExctractReportTableCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Заполнить инд. план",
                Description = "Получает на вход файл word с индивидуальным планом и выдаёт его версию с заполненной таблицей",
                Command = FillIndividualPlanCommand
            });
        }

        [RelayCommand(CanExecute = nameof(NoTask))]
        public async Task LoadSession()
        {
            var page = new DataPageViewModel();
            await CommandWithProcessAndLoad(page.LoadLessonData, page);
        }

        [RelayCommand(CanExecute = nameof(NoTask))]
        public async Task LoadExcel()
        {
            var page = new DataPageViewModel();

            var path = GetFile("excel file|*.xlsx");

            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            await CommandWithProcessAndLoad(async () =>
            {
                await Ioc.Default.GetRequiredService<IDataReader>().ReadToData(path);
                await page.LoadLessonData();
            }, page);
        }

        [RelayCommand(CanExecute = nameof(NoTask))]
        public async Task LoadTempFile()
        {
            var page = new DataPageViewModel();

            var path = GetFile("excel file|*.xlsx");

            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            await CommandWithProcessAndLoad(async () =>
            {
                await Ioc.Default.GetRequiredService<ITempFileWorker>().ReadFile(path);
                await page.LoadLessonData();
            }, page);
        }

        [RelayCommand]
        public async Task UploadCalculation()
        {
            var path = GetFile("excel file|*.xlsx");

            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            await CommandWithProcess(async () =>
            {
                await Ioc.Default.GetRequiredService<IEducationReader>().ReadFromFile(path);
            },
            async () =>
            {
                await Notify("Данные загружены!", "Теперь вы можете заполнять индивидуальные планы и формировать отчёт!");
                IsLoadCalculation = true;
            },
            async () =>
            {
                await Notify("Ошибка выгрузки!", "Что-то пошло не так.", NotifyColorType.Error);
            });
        }

        [RelayCommand(CanExecute = nameof(CanExtract))]
        public async Task ExtractToTempFile()
        {
            var path = GetFile("excel file|*.xlsx", "временный файл.xlsx");
            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            var page = new DataPageViewModel();

            await CommandWithProcessAndLoad(async () =>
            {
                await Ioc.Default.GetRequiredService<ITempFileWorker>().WriteFile(path);
                await page.LoadLessonData();
            }, page, "Теперь, вы можете обновить файл и загрузить его с помощью команты \"Загрузить временный файл\"!");
        }

        [RelayCommand(CanExecute = nameof(CanExtract))]
        public async Task ExctractWorkloadTable()
        {
            var page = new DataPageViewModel();

            var defFileName = "Загруженность.xlsx";
            var fileDialog = new SaveFileDialog
            {
                Filter = "excel file|*.xlsx",
                FileName = defFileName
            };
            fileDialog.ShowDialog();

            if (string.IsNullOrEmpty(fileDialog.FileName) || fileDialog.FileName == defFileName)
                return;

            await CommandWithProcessAndLoad(async () =>
            {
                await Ioc.Default.GetRequiredService<IWorkSchedule>().Write(fileDialog.FileName);
                await page.LoadLessonData();
            }, page, "График готов, файл создан!");
        }

        [RelayCommand(CanExecute = nameof(CanExtract))]
        public async Task ExctractReportTable()
        {
            var page = new DataPageViewModel();

            var path = GetFile("excel file|*.xlsx", "Отчёт по месяцам и дисциплинам.xlsx");

            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            await CommandWithProcessAndLoad(async () =>
            {
                await Ioc.Default.GetRequiredService<IReportWriter>().Write(path);
                await page.LoadLessonData();
            }, page, "Отчёт готов, файл создан!");
        }

        [RelayCommand(CanExecute = nameof(IsLoadCalculation))]
        public async Task FillIndividualPlan()
        {
            var path = GetFile("word file|*.docx");
            
            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            if (TeacherName ==  null)
            {
                await Notify("Нет имени преподавателя!", "Установите его в левом нижнем углу.", NotifyColorType.Error);
                return;
            }

            await CommandWithProcess(async () =>
            {
                await Ioc.Default.GetRequiredService<IIndividualPlanWriter>().FillPlan(TeacherName, path);
            },
            async () =>
            {
                await Notify("Данные выгружены!", "Индивидуальные планы готовы, файлы созданы!");
            },
            async () =>
            {
                await Notify("Ошибка выгрузки!", "Что-то пошло не так.", NotifyColorType.Error);
            });
        }
        private static string? GetFile(string filter, string? defaultFileName = null)
        {
            var calculationFilePath = new OpenFileDialog { Filter = filter };

            if (defaultFileName != null)
                calculationFilePath.FileName = defaultFileName;

            calculationFilePath.ShowDialog();
            return defaultFileName != calculationFilePath.FileName ? calculationFilePath.FileName : null;
        }

        private async Task CommandWithProcessAndLoad(Func<Task> action, object? page = null, string msg = "Теперь, вы можете использывать другие команды!")
        {
            await CommandWithProcessAndError(action, async () =>
            {
                await Notify("Данные загружены!", msg);
                CurPage = page;
                CanExtract = true;
            });
        }

        private async Task CommandWithProcessAndError(Func<Task> action, Action? onSucces = null)
        {
            await CommandWithProcess(action, onSucces, async () =>
            {
                await Notify("Ошибка загрузки!", "Что-то пошло не так.", NotifyColorType.Error);
            });
        }

        private async Task CommandWithProcess(Func<Task> action, Action? onSucces = null, Action? onProblem = null)
        {
            var lp = new LoadingPageViewModel();
            CurPage = lp;

            var task = Task.Run(() =>
            {

#if RELEASE
                try
                {
#endif
                action.Invoke();
#if RELEASE
                }
                catch
                {
                    return false;
                }
#endif
                return true;
            });

            CurTask = task;
            NoTask = false;

            var res = await task;

            if (res)
                onSucces?.Invoke();
            else
                onProblem?.Invoke();

            if (CurPage == lp)
                CurPage = null;

            CurTask = null;
            NoTask = true;
        }

        private async Task NoHasFileMessage()
            => await Notify("Неудалось выгрузить!", "Вы не указали путь до файла!", NotifyColorType.Warning);

        private async Task NoHasDirrectoryMessage()
            => await Notify("Неудалось выгрузить!", "Вы не указали путь до файла!", NotifyColorType.Warning);

        private Task Notify(string title, string desc, NotifyColorType colorType = NotifyColorType.Info)
        {
            var notify = Ioc.Default.GetRequiredService<NotifyBuilder>()
                .WithTitle(title)
                .WithDescription(desc)
                .WithColor(colorType)
                .WithRemove(Notifies.Remove)
                .Build();

            Notifies.Insert(0, notify);
            return Task.CompletedTask;
        }
    }
}
