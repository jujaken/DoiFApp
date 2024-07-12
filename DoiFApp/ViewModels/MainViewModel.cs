using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Services;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using FolderBrowserEx;

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
        public bool noTask = true;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ExtractToTempFileCommand),
                                    nameof(ExctractWorkloadTableCommand),
                                    nameof(ExctractReportTableCommand))]
        public bool canExtract;

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
                Description = "Формирует таблицу-отчёт",
                Command = ExctractReportTableCommand
            });

            tools.Add(new ToolViewModel()
            {
                Title = "Выдать инд. планы",
                Description = "Выдаёт файлы индивидуальных планов",
                Command = ExctractIndividualPlansCommand
            });
        }

        [RelayCommand(CanExecute = nameof(NoTask))]
        public async Task LoadSession()
        {
            var page = new DataPageViewModel();

            await CommandWithProcess(async () =>
            {
                await page.LoadLessonData();
                try
                {
                }
                catch
                {
                    return false;
                }
                return true;
            },
             async () =>
             {
                 await Notify("Данные загружены!", "Теперь, вы можете использывать другие команды!");
                 CurPage = page;
                 CanExtract = true;
             },
             async () =>
             {
                 await Notify("Ошибка загрузки!", "Что-то пошло не так.", NotifyColorType.Error);
             });
        }

        [RelayCommand(CanExecute = nameof(NoTask))]
        public async Task LoadExcel()
        {
            var page = new DataPageViewModel();

            var fileDialog = new OpenFileDialog
            {
                Filter = "excel file|*.xlsx"
            };

            fileDialog.ShowDialog();

            if (string.IsNullOrEmpty(fileDialog.FileName))
                return;

            await CommandWithProcess(async () =>
            {
                await Ioc.Default.GetRequiredService<IDataReader>().ReadToData(fileDialog.FileName);
                await page.LoadLessonData();
                try
                {
             
                }
                catch
                {
                    return false;
                }
                return true;
            },
            async () =>
            {
                await Notify("Данные загружены!", "Теперь, вы можете использывать другие команды!");
                CurPage = page;
                CanExtract = true;
            },
            async () =>
            {
                await Notify("Ошибка загрузки!", "Что-то пошло не так.", NotifyColorType.Error);
            });
        }

        [RelayCommand(CanExecute = nameof(NoTask))]
        public async Task LoadTempFile()
        {
            var page = new DataPageViewModel();

            var fileDialog = new OpenFileDialog
            {
                Filter = "excel file|*.xlsx"
            };

            fileDialog.ShowDialog();

            if (string.IsNullOrEmpty(fileDialog.FileName))
                return;

            await CommandWithProcess(async () =>
            {
                await Ioc.Default.GetRequiredService<ITempFileWorker>().ReadFile(fileDialog.FileName);
                await page.LoadLessonData();
                try
                {
             
                }
                catch
                {
                    return false;
                }
                return true;
            },
            async () =>
            {
                await Notify("Данные выгружены!", "Теперь, вы можете обновить файл и загрузить его с помощью команты \"Загрузить временный файл\"!");
                CurPage = page;
                CanExtract = true;
            },
            async () =>
            {
                await Notify("Ошибка загрузки!", "Что-то пошло не так.", NotifyColorType.Error);
            });
        }

        [RelayCommand(CanExecute = nameof(CanExtract))]
        public async Task ExtractToTempFile()
        {
            var page = new DataPageViewModel();

            var defFileName = "временный файл.xlsx";
            var fileDialog = new SaveFileDialog
            {
                Filter = "excel file|*.xlsx",
                FileName = defFileName
            };
            fileDialog.ShowDialog();

            if (string.IsNullOrEmpty(fileDialog.FileName) || fileDialog.FileName == defFileName)
                return;

            await CommandWithProcess(async () =>
            {

                await Ioc.Default.GetRequiredService<ITempFileWorker>().WriteFile(fileDialog.FileName);
                await page.LoadLessonData();
                try
                {
                }
                catch
                {
                    return false;
                }
                return true;
            },
            async () =>
            {
                await Notify("Данные выгружены!", "Теперь, вы можете обновить файл и загрузить его с помощью команты \"Загрузить временный файл\"!");
                CurPage = page;
                CanExtract = true;
            },
            async () =>
            {
                await Notify("Ошибка выгрузки!", "Что-то пошло не так.", NotifyColorType.Error);
            });
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

            await CommandWithProcess(async () =>
            {
                await Ioc.Default.GetRequiredService<IWorkSchedule>().Write(fileDialog.FileName);
                await page.LoadLessonData();
                try
                {
               
                }
                catch
                {
                    return false;
                }
                return true;
            },
            async () =>
            {
                await Notify("Данные выгружены!", "График готов, файл создан!");
                CurPage = page;
                CanExtract = true;
            },
            async () =>
            {
                await Notify("Ошибка выгрузки!", "Что-то пошло не так.", NotifyColorType.Error);
            });
        }

        [RelayCommand(CanExecute = nameof(CanExtract))]
        public async Task ExctractReportTable()
        {
            var page = new DataPageViewModel();

            var defFileName = "Отчёт по месяцам и дисциплинам.xlsx";
            var fileDialog = new SaveFileDialog
            {
                Filter = "excel file|*.xlsx",
                FileName = defFileName
            };
            fileDialog.ShowDialog();

            if (string.IsNullOrEmpty(fileDialog.FileName) || fileDialog.FileName == defFileName)
                return;

            await CommandWithProcess(async () =>
            {
                await Ioc.Default.GetRequiredService<IReportWriter>().Write(fileDialog.FileName);
                await page.LoadLessonData();
                try
                {
          
                }
                catch
                {
                    return false;
                }
                return true;
            },
            async () =>
            {
                await Notify("Данные выгружены!", "Отчёт готов, файл создан!");
                CurPage = page;
                CanExtract = true;
            },
            async () =>
            {
                await Notify("Ошибка выгрузки!", "Что-то пошло не так.", NotifyColorType.Error);
            });
        }

        private readonly string lastDirectory = Environment.CurrentDirectory;

        [RelayCommand]
        public async Task ExctractIndividualPlans()
        {
            var inputDialog = new OpenFileDialog
            {
                Filter = "excel file|*.xlsx",
            };
            inputDialog.ShowDialog();
            if (string.IsNullOrEmpty(inputDialog.FileName))
            {
                await Notify("Неудалось загрузить!", "Вы не указали файл отчёта!", NotifyColorType.Warning);
                return;
            }

            var outputDialog = new FolderBrowserDialog()
            {
                DefaultFolder = lastDirectory
            };
            outputDialog.ShowDialog();
            if (string.IsNullOrEmpty(outputDialog.SelectedFolder))
            {
                await Notify("Неудалось выгрузить!", "Вы не указали путь парки для индивидуальных планов!", NotifyColorType.Warning);
                return;
            }

            await CommandWithProcess(async () =>
            {
                await Ioc.Default.GetRequiredService<IEducationReader>().ReadFromFile(inputDialog.FileName);
                await Ioc.Default.GetRequiredService<IIndividualPlanWriter>().MakePlans(outputDialog.SelectedFolder);
                try
                {
                }
                catch
                {
                    return false;
                }
                return true;
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

        private async Task CommandWithProcess(Func<Task<bool>> action, Action? onSucces = null, Action? onProblem = null)
        {
            var lp = new LoadingPageViewModel();
            CurPage = lp;

            var task = Task.Run(action);

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
