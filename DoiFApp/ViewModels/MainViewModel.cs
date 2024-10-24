﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Data;
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services;
using DoiFApp.Services.Builders;
using DoiFApp.Services.Data;
using DoiFApp.Services.Education;
using DoiFApp.Services.IndividualPlan;
using DoiFApp.Services.Schedule;
using DoiFApp.Services.TempSchedule;
using DoiFApp.Services.Workload;
using DoiFApp.ViewModels.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Shapes;

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

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RemoveDbCommand))]
        public bool canRemoveDb = true;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ExtractTempScheduleCommand))]
        [NotifyCanExecuteChangedFor(nameof(ExtractWorkloadCommand))]
        [NotifyCanExecuteChangedFor(nameof(CheckScheduleCommand))]
        [NotifyCanExecuteChangedFor(nameof(FromReportCommand))]
        public bool scheduleIsLoad = false;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FillIndividualPlanCommand))]
        public bool educationIsLoad = false;

        #endregion

        public MainViewModel()
        {
            var noCommand = new RelayCommand(() => { }, () => false);

            // общее

            var loadSchedule = new ToolViewModel()
            {
                Title = "📅 Загрузить расписание",
                Description = "Загружает таблицу excel с расписанием и формирует необходимые данные для работы приложения",
                Command = LoadScheduleCommand,
            };

            // плановая

            var fillIndividualPlan = new ToolViewModel()
            {
                Title = "📝 Заполнить инд. план",
                Description = "Позволяет выбрать преподавателя и заполняет данный word файл",
                Command = FillIndividualPlanCommand
            };

            var loadCalculation = new ToolViewModel()
            {
                Title = "📊 Загрузить расчёт уч. нагрузки",
                Description = "Позволяет выбрать преподавателя и заполняет данный word файл",
                Command = LoadCalculationCommand
            };

            var loadMethodicalWork = new ToolViewModel()
            {
                Title = "📚 Загрузить метод. работу",
                Description = "Загружает методическую работу из word файла",
                Command = noCommand
            };

            var loadScientificWork = new ToolViewModel()
            {
                Title = "🔬 Загрузить науч. работу",
                Description = "Загружает научную работу из word файла",
                Command = noCommand
            };

            var moralMentalWork = new ToolViewModel()
            {
                Title = "🧠 Загрузить мор.-псих. работу",
                Description = "Загружает морально-психологическую работу из word файла",
                Command = noCommand
            };

            var foreignersWork = new ToolViewModel()
            {
                Title = "🌍 Загруж. работу с иностранн. слуш.",
                Description = "Загружает работу с иностранными слушателями из word файла",
                Command = noCommand
            };

            var otherWork = new ToolViewModel()
            {
                Title = "📁 Загрузить другую работу",
                Description = "Загружает иные виды работ из word файла",
                Command = noCommand
            };

            var plan = new ToolCategoryViewModel("Плановая нагрузка",
                fillIndividualPlan,
                loadCalculation,
                loadMethodicalWork,
                loadScientificWork,
                moralMentalWork,
                foreignersWork,
                otherWork);

            // фактическая

            var loadReport = new ToolViewModel()
            {
                Title = "📈 Загрузить отчёт",
                Description = "Загружает отчёт из excel файла",
                Command = noCommand
            };

            var exportReportToIP = new ToolViewModel()
            {
                Title = "📤 Выгрузить отчёт в индивидуальный план",
                Description = "Загружает отчёт в word файл",
                Command = noCommand
            };

            var formReportByMW = new ToolViewModel()
            {
                Title = "📅 Сформировать отчет по месячной нагрузке",
                Description = "Загружает отчёт по месяцам в word файл",
                Command = noCommand
            };

            var fillReportMW = new ToolViewModel()
            {
                Title = "✏️ Заполнить ежемес. нагрузку",
                Description = "Заполняет ежемесячную нагрузку",
                Command = noCommand
            };

            var fact = new ToolCategoryViewModel("Фактическая нагрузка",
                loadReport,
                exportReportToIP,
                loadSchedule,
                formReportByMW,
                fillReportMW);

            toolsCategories.Add(new ToolCategoryViewModel("Индивидуальный план",
                plan,
                fact));

            // отчетное

            var checkSchedule = new ToolViewModel()
            {
                Title = "🔍 Проверить расписание",
                Description = "Позволяет просмотреть виды занятия и правильности их обозначения",
                Command = CheckScheduleCommand
            };

            var fromReport = new ToolViewModel()
            {
                Title = "📄 Сформировать отчёт",
                Description = "Формулирует и выгружает данные для отчёта в excel",
                Command = FromReportCommand
            };

            toolsCategories.Add(new ToolCategoryViewModel("Отчётная документация",
                loadSchedule,
                checkSchedule,
                fromReport
            ));

            // загруженность

            var extractTempSchedule = new ToolViewModel()
            {
                Title = "🖊️ Выдать редакт. расписание",
                Description = "Выгружает из сессии имеющееся расписание, позволяет отредактировать его вручную в excel",
                Command = ExtractTempScheduleCommand
            };

            var loadTempSchedule = new ToolViewModel()
            {
                Title = "📥 Загр. редакт. расписание",
                Description = "Загружает excel файл с отредактированным расписанием",
                Command = LoadTempScheduleCommand
            };

            var extractWorkload = new ToolViewModel()
            {
                Title = "📊 Выдать загруженность",
                Description = "Формирует таблицу загруженности",
                Command = ExtractWorkloadCommand
            };

            toolsCategories.Add(new ToolCategoryViewModel("Загруженность преподавателей",
                loadSchedule,
                extractWorkload,
                extractTempSchedule,
                loadTempSchedule
            ));

            // doif

            var loadLastSession = new ToolViewModel()
            {
                Title = "📂 Загрузить предыдущую сессию",
                Description = "Загружает сессию из файла doifapp.db",
                Command = LoadLastSessionCommand
            };

            var clearSession = new ToolViewModel()
            {
                Title = "🧹 Очистить сессию",
                Description = "Очищает все собранные данные из сессии",
                Command = ClearSessionCommand
            };

            var importSession = new ToolViewModel()
            {
                Title = "📥 Загрузить файл сессии",
                Description = "Загружает файл сессии",
                Command = ImportSessionCommand
            };

            var exportSession = new ToolViewModel()
            {
                Title = "📤 Выгрузить файл сессии",
                Description = "Выгружает файл сессии",
                Command = ExportSessionCommand
            };

            var removeDb = new ToolViewModel()
            {
                Title = "🗑️ Удалить данные",
                Description = "Удаляет файл с сессей. Команда доступна только как первая сразу после запуска",
                Command = RemoveDbCommand
            };

            var clearNotifies = new ToolViewModel()
            {
                Title = "🧹 Очистить уведомления",
                Description = "Убирает уведомления справа",
                Command = ClearNotifiesCommand
            };

            toolsCategories.Add(new ToolCategoryViewModel("DoiF",
                loadLastSession,
                clearSession,
                importSession,
                exportSession,
                removeDb,
                clearNotifies
                ));
        }

        #region Common

        [RelayCommand(CanExecute = nameof(NoTask))]
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
                await page.LoadData();
            }, page);

            if (page.LessonViewModels.Any())
                ScheduleIsLoad = true;
        }

        #endregion

        #region Плановая нагрузка

        [RelayCommand(CanExecute = nameof(EducationIsLoad))]
        public async Task FillIndividualPlan()
        {
            var page = new FillIndividualPlanPageViewModel();
            page.OnCancel += () => CurPage = null;
            page.OnOk += async (result) =>
            {
                var path = GetFile("word file|*.docx", "Индивидуальный план.docx");
                if (string.IsNullOrEmpty(path))
                {
                    await NoHasFileMessage();
                    return;
                }

                var dataPage = new DataPageViewModel();
                await CommandWithProcessAndLoad(async () =>
                {
                    var teacher = (await Ioc.Default.GetRequiredService<IRepo<EducationTeacherModel>>()
                        .Include(at => at.Works)
                        .GetWhere(t => t.Name == result.teacherName)).FirstOrDefault();

                    if (result.isFirstSemester)
                    {
                        var data = new FirstHalfIndividualPlanData() { TeacherModel = teacher };
                        await Ioc.Default.GetRequiredService<IDataWriter<FirstHalfIndividualPlanData>>().Write(data, path);
                    }
                    else
                    {
                        var data = new SecondHalfIndividualPlanData() { TeacherModel = teacher };
                        await Ioc.Default.GetRequiredService<IDataWriter<SecondHalfIndividualPlanData>>().Write(data, path);
                    }
                    await dataPage.LoadData();
                }, dataPage, "Задание выполнено");
            };
            await CommandWithProcessAndLoad(page.Update, page, "Меню открыто");
        }

        [RelayCommand(CanExecute = nameof(NoTask))]
        public async Task LoadCalculation()
        {
            var path = GetFile("excel file|*.xlsx", "Расчёт.xlsx");
            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            var page = new DataPageViewModel();

            await CommandWithProcessAndLoad(async () =>
            {
                var data = await Ioc.Default.GetRequiredService<IDataReader<EducationData>>().Read(path);
                if (!data.IsHolistic)
                    throw new Exception("Data not found");

                await Ioc.Default.GetRequiredService<IDataSaver<EducationData>>().Save(data);
                await page.LoadData();
            }, page, "Данные из расчёта расписания были загружены");

            if (page.LessonViewModels.Any())
                EducationIsLoad = true;
        }

        #endregion

        #region Фактическая нагрузка

        #endregion

        #region Отчётная документация

        [RelayCommand(CanExecute = nameof(ScheduleIsLoad))]
        private async Task CheckSchedule()
        {
            var page = new CheckSchedulePageViewModel();
            page.OnCancel += () => CurPage = null;
            page.OnOk += async (lessonTypeTranslations) =>
            {
                var dataPage = new DataPageViewModel();
                await CommandWithProcessAndLoad(async () =>
                {
                    var repo = Ioc.Default.GetRequiredService<IRepo<LessonModel>>();
                    (await repo.GetAll()).ForEach(l =>
                    {
                        l.LessionType = lessonTypeTranslations.FirstOrDefault(t => t.CurrentName == l.LessionType)!.NewName;
                        repo.Update(l);
                    });
                    await dataPage.LoadData();
                }, dataPage, "Задание выполнено");
            };
            await CommandWithProcessAndLoad(page.Update, page, "Меню открыто");
        }

        [RelayCommand(CanExecute = nameof(ScheduleIsLoad))]
        private async Task FromReport()
        {
            var path = SaveFile("excel file|*.xlsx", "Отчёт по месяцам и дисциплинам.xlsx");
            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            var page = new DataPageViewModel();

            await CommandWithProcessAndLoad(async () =>
            {
                var data = await Ioc.Default.GetRequiredService<IRepo<LessonModel>>().GetAll();
                await Ioc.Default.GetRequiredService<IDataWriter<ScheduleData>>().Write(new() { Lessons = data }, path);
                await page.LoadData();
            }, page, "Отчёт готов, файл создан!");
        }

        #endregion

        #region Загруженность преподавателей

        [RelayCommand(CanExecute = nameof(ScheduleIsLoad))]
        public async Task ExtractTempSchedule()
        {
            var path = SaveFile("excel file|*.xlsx", "редактируемое расписание.xlsx");
            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            var page = new DataPageViewModel();

            await CommandWithProcessAndLoad(async () =>
            {
                var scheduleData = await Ioc.Default.GetRequiredService<IRepo<LessonModel>>().GetAll();
                await Ioc.Default.GetRequiredService<IDataWriter<TempScheduleData>>().Write(new() { Lessons = scheduleData }, path);
            }, page, "Теперь, вы можете обновить файл и загрузить его с помощью команты \"Загр. редакт. расписание\"!");
        }

        [RelayCommand(CanExecute = nameof(NoTask))]
        public async Task LoadTempSchedule()
        {
            var path = GetFile("excel file|*.xlsx", "редактируемое расписание.xlsx");
            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            var page = new DataPageViewModel();

            await CommandWithProcessAndLoad(async () =>
            {
                var data = await Ioc.Default.GetRequiredService<IDataReader<TempScheduleData>>().Read(path);
                if (data.Lessons == null || !data.Lessons.Any())
                    throw new Exception("Data not found");

                await Ioc.Default.GetRequiredService<IDataSaver<TempScheduleData>>().Save(data);
            }, page, "Данные из редактируемого расписания были загружены");

            if (page.LessonViewModels.Any())
                ScheduleIsLoad = true;
        }

        [RelayCommand(CanExecute = nameof(ScheduleIsLoad))]
        public async Task ExtractWorkload()
        {
            var res = MessageBox.Show("Желаете выбрать месяцы?", "Режим вывода", MessageBoxButtons.YesNo);

            if (res == DialogResult.Yes)
            {
                var page = new ExtractWorkloadPageViewModel();
                page.OnCancel += () => CurPage = null;
                page.OnOk += ExtractWorkload;
                CurPage = page;
            }

            else
                await ExtractWorkload(Enumerable.Range(1, 12).ToArray());
        }

        public async Task ExtractWorkload(int[] months)
        {
            var path = SaveFile("excel file|*.xlsx", "Загруженность.xlsx");
            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            var page = new DataPageViewModel();

            await CommandWithProcessAndLoad(async () =>
            {
                var data = await Ioc.Default.GetRequiredService<IRepo<LessonModel>>().GetWhere(x => months.Contains(x.Date.Month));
                await Ioc.Default.GetRequiredService<IDataWriter<WorkloadData>>().Write(new() { Lessons = data }, path);
                await page.LoadData();
            }, page, "График загруженности готов, файл создан!");
        }

        #endregion

        #region DoiF Commands

        [RelayCommand(CanExecute = nameof(NoTask))]
        private async Task ClearSession()
        {
            if (!File.Exists(App.DbPath))
            {
                await Notify("Сессия не загружена", $"Невозможно загрузить сессию, так как остутствует файл {App.DbPath}", NotifyColorType.Error);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите очистить сессию?", "Подтверждение", MessageBoxButtons.YesNo);
            if (result != DialogResult.Yes)
            {
                await Notify("Отмена очистки", "Сессия не была очищена");
                return;
            }

            await CommandWithProcessAndError(async () =>
            {
                var db = Ioc.Default.GetRequiredService<AppDbContext>();
                db.RecreateLessons();
                db.RecreateEducation();
                await db.SaveChangesAsync();
            }, async () =>
            {
                await Notify("Данные очищены", "Данные сессии очищены!");
                return string.Empty; // это небольшой костыль, лучше бы исправить
            });
        }

        [RelayCommand(CanExecute = nameof(NoTask))]
        private async Task LoadLastSession()
        {
            var page = new DataPageViewModel();
            if (!File.Exists(App.DbPath))
            {
                await Notify("Сессия не загружена", $"Невозможно загрузить сессию, так как остутствует файл {App.DbPath}", NotifyColorType.Error);
                return;
            }
            await CommandWithProcessAndLoad(page.LoadData, page);

            if (page.LessonViewModels.Any())
                ScheduleIsLoad = true;

            if (page.EducationTeacherModel.Any())
                EducationIsLoad = true;
        }

        [RelayCommand(CanExecute = nameof(NoTask))]
        private async Task ImportSession()
        {
            var path = GetFile("session file|*.db");

            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            var page = new DataPageViewModel();

            await CommandWithProcessAndLoad(async () =>
            {
                await Ioc.Default.GetRequiredService<IDbCopier>().Copy(path, App.DbPath);
                await page.LoadData();
            }, page);
        }

        [RelayCommand(CanExecute = nameof(NoTask))]
        private async Task ExportSession()
        {
            var path = SaveFile("database|*.db", "doifapp-session.db");

            if (string.IsNullOrEmpty(path))
            {
                await NoHasFileMessage();
                return;
            }

            await CommandWithProcessAndError(() =>
            {
                File.Delete(path);
                File.Copy(App.DbPath, path);

                return Task.CompletedTask;
            }, async () =>
            {
                await Notify("Сессия выгружена", "Данные из сессии успешно скопированы в отдельный файл");
                return null;
            });
        }

        [RelayCommand(CanExecute = nameof(CanRemoveDb))]
        private async Task RemoveDb()
        {
            await CommandWithProcessAndError(() =>
            {
                File.Delete(App.DbPath);
                return Task.CompletedTask;
            }, async () =>
            {
                await Notify("Данные удалены", "Данные из сессии успешно удалены");
                return null;
            });
        }

        [RelayCommand]
        private Task ClearNotifies()
        {
            var oldPage = CurPage;
            CurPage = null;
            Notifies.Clear();
            CurPage = oldPage;
            return Task.CompletedTask;
        }


        #endregion

        #region File Tools

        public static string? GetFile(string filter, string? defaultFileName = null)
        {
            var calculationFilePath = new OpenFileDialog { Filter = filter };

            if (defaultFileName != null)
                calculationFilePath.FileName = defaultFileName;

            calculationFilePath.ShowDialog();
            return defaultFileName != calculationFilePath.FileName ? calculationFilePath.FileName : null;
        }

        public static string? SaveFile(string filter, string? defaultFileName = null)
        {
            var calculationFilePath = new SaveFileDialog { Filter = filter };

            if (defaultFileName != null)
                calculationFilePath.FileName = defaultFileName;

            calculationFilePath.ShowDialog();
            return defaultFileName != calculationFilePath.FileName ? calculationFilePath.FileName : null;
        }

        #endregion

        #region Command Tools

        public async Task CommandWithProcessAndLoad(Func<Task> action, object? page = null, string msg = "Теперь, вы можете использывать другие команды!")
        {
            await CommandWithProcessAndError(action, async () =>
            {
                await Notify("Данные загружены!", msg);
                CanExtract = true;
                return page;
            });
        }

        public async Task CommandWithProcessAndError(Func<Task> action, Func<Task<object?>>? onSucces = null)
        {
            await CommandWithProcess(action, onSucces, async () =>
            {
                await Notify("Ошибка!", "Что-то пошло не так.", NotifyColorType.Error);
                return null;
            });
        }

        public async Task CommandWithProcess(Func<Task> action, Func<Task<object?>>? onSucces = null, Func<Task<object?>>? onProblem = null)
        {
            var oldPage = CurPage;
            CurPage = new LoadingPageViewModel();

#if RELEASE
            try
            {
#endif
            await Task.Run(action.Invoke);
            CurPage = onSucces == null ? null : await onSucces.Invoke() ?? oldPage;
#if RELEASE
            }
            catch
            {
                CurPage = onProblem == null ? null : await onProblem.Invoke();
            }
#endif
            CurTask = null;
            NoTask = true;

            CanRemoveDb = false;
        }

        #endregion

        #region Notify tools

        public async Task NoHasFileMessage()
         => await Notify("Неудалось выгрузить!", "Вы не указали путь до файла!", NotifyColorType.Warning);

        public async Task NoHasDirrectoryMessage()
            => await Notify("Неудалось выгрузить!", "Вы не указали путь до файла!", NotifyColorType.Warning);

        public Task Notify(string title, string desc, NotifyColorType colorType = NotifyColorType.Info)
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
