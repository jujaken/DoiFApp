using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Services.Builders;
using DoiFApp.Services.Data;
using DoiFApp.Services.Schedule;
using DoiFApp.ViewModels.Pages;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace DoiFApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ToolCategoryViewModel> toolsCategories = [];

        [ObservableProperty]
        private ObservableCollection<NotifyViewModel> notifies = [];

        [ObservableProperty]
        private object? curPage;

        [ObservableProperty]
        private Task? curTask;

        #region Conditions 

        [ObservableProperty]
        private bool noTask = true;

        [ObservableProperty]
        private bool canExtract;

        #endregion

        public MainViewModel()
        {
            var loadSchedule = new ToolViewModel()
            {
                Title = "Загрузить расписание",
                Description = "Загружает таблицу excel с расписанием и формирует необходимые данные для работы приложения",
                Command = LoadScheduleCommand,
            };

            var checkSchedule = new ToolViewModel()
            {
                Title = "Проверить расписание",
                Description = "Позволяет просмотреть виды занятия и правильности их обозначения",
                Command = null
            };

            var fillIndividualPlan = new ToolViewModel()
            {
                Title = "Заполнить инд. план",
                Description = "Позволяет выбрать преподавателя и заполняет данный word файл",
                Command = null
            };

            var loadCalculation = new ToolViewModel()
            {
                Title = "Загрузить расчёт уч. нагрузки",
                Description = "Позволяет выбрать преподавателя и заполняет данный word файл",
                Command = null
            };

            var loadMethodicalWork = new ToolViewModel()
            {
                Title = "Загрузить метод. работу",
                Description = "Загружает методическую работу из word файла",
                Command = null
            };

            var loadScientificWork = new ToolViewModel()
            {
                Title = "Загрузить науч. работу",
                Description = "Загружает научную работу из word файла",
                Command = null
            };

            var moralMentalWork = new ToolViewModel()
            {
                Title = "Загрузить мор.-псих. работу",
                Description = "Загружает морально-психологическую работу из word файла",
                Command = null
            };

            var foreignersWork = new ToolViewModel()
            {
                Title = "Загруз. работу с иностранн. слуш.",
                Description = "Загружает работу с иностранными слушателями из word файла",
                Command = null
            };

            var otherWork = new ToolViewModel()
            {
                Title = "Загрузить другую работу",
                Description = "Загружает иные виды работ из word файла",
                Command = null
            };

            toolsCategories.Add(new ToolCategoryViewModel("Индивидуальный план",
                fillIndividualPlan,
                loadCalculation,
                loadMethodicalWork,
                loadScientificWork,
                moralMentalWork,
                foreignersWork,
                otherWork
                ));

            // todo fact logic

            toolsCategories.Add(new ToolCategoryViewModel("Фактическая нагрузка"));

            var fromReport = new ToolViewModel()
            {
                Title = "Сформировать отчёт",
                Description = "Формулирует и выгружает данные для отчёта в excel",
                Command = null
            };

            toolsCategories.Add(new ToolCategoryViewModel("Отчётная документация",
                loadSchedule,
                checkSchedule,
                fromReport
                ));

            var extractWorkload = new ToolViewModel()
            {
                Title = "Выдать загруженность",
                Description = "Формирует таблицу загруженности",
                Command = null
            };

            toolsCategories.Add(new ToolCategoryViewModel("Загруженность преподавателей",
                loadSchedule,
                checkSchedule,
                extractWorkload
                ));

            var loadLastSession = new ToolViewModel()
            {
                Title = "Загрузить предыдущую сессию",
                Description = "Загружает сессию из файла doifapp.db",
                Command = null
            };

            var exitSession = new ToolViewModel()
            {
                Title = "Выйти из сессии",
                Description = "Выходит из сессии",
                Command = null
            };

            var clearSession = new ToolViewModel()
            {
                Title = "Очистить сессию",
                Description = "Очищает все собранные данные из сессии",
                Command = null
            };

            var importSession = new ToolViewModel()
            {
                Title = "Загрузить файл сессии",
                Description = "Загружает файл сессии",
                Command = null
            };

            var exportSession = new ToolViewModel()
            {
                Title = "Выгрузить файл сессии",
                Description = "Выгружает файл сессии",
                Command = null
            };

            toolsCategories.Add(new ToolCategoryViewModel("DoiF",
                loadLastSession,
                exitSession,
                clearSession,
                importSession,
                exportSession
                ));
        }

        [RelayCommand]
        private async Task LoadSchedule()
        {
            var path = GetFile("excel file|*.xlsx");

            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            var page = new DataPageViewModel();

            await CommandWithProcessAndLoad(async () =>
            {
                var data = await Ioc.Default.GetRequiredService<IDataReader<ScheduleData>>().Read(path);
                if (data.Lessons == null || !data.Lessons.Any())
                    throw new Exception("Data not found");

                await Ioc.Default.GetRequiredService<IDataSaver<ScheduleData>>().Save(data);
                await page.LoadLessonData();
            }, page);
        }

        private static string? GetFile(string filter, string? defaultFileName = null)
        {
            var calculationFilePath = new OpenFileDialog { Filter = filter };

            if (defaultFileName != null)
                calculationFilePath.FileName = defaultFileName;

            calculationFilePath.ShowDialog();
            return defaultFileName != calculationFilePath.FileName ? calculationFilePath.FileName : null;
        }

        #region Command Tools

        private async Task CommandWithProcessAndLoad(Func<Task> action, object? page = null, string msg = "Теперь, вы можете использывать другие команды!")
        {
            await CommandWithProcessAndError(action, async () =>
            {
                await Notify("Данные загружены!", msg);
                CanExtract = true;
                return page;
            });
        }

        private async Task CommandWithProcessAndError(Func<Task> action, Func<Task<object?>>? onSucces = null)
        {
            await CommandWithProcess(action, onSucces, async () =>
            {
                await Notify("Ошибка загрузки!", "Что-то пошло не так.", NotifyColorType.Error);
                return null;
            });
        }

        private async Task CommandWithProcess(Func<Task> action, Func<Task<object?>>? onSucces = null, Func<Task<object?>>? onProblem = null)
        {
            var oldPage = CurPage;
            CurPage = new LoadingPageViewModel();

#if RELEASE
            try
            {
#endif
                await Task.Run(action.Invoke);
                CurPage = onSucces == null ? null : await onSucces.Invoke();
#if RELEASE
            }
            catch
            {
                CurPage = onProblem == null ? null : await onProblem.Invoke();
            }
#endif
            CurTask = null;
            NoTask = true;
        }

        #endregion

        #region Notify tools

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

        #endregion
    }
}
