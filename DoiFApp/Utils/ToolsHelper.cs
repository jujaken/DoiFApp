using CommunityToolkit.Mvvm.ComponentModel;
using DoiFApp.ViewModels;
using System.Collections.ObjectModel;

namespace DoiFApp.Utils
{
    public static class ToolsHelper
    {
        public static void AddDefaults(MainViewModel main, ObservableCollection<ToolCategoryViewModel> tools)
        {
            var loadSession = new ToolViewModel()
            {
                Title = "Загрузить из сессии",
                Description = "Загружает таблицу excel и формирует необходимые данные для работы приложения",
                Command = main.LoadSessionCommand
            };

            var loadExcelSchedule = new ToolViewModel()
            {
                Title = "Загрузить расписание",
                Description = "Загружает таблицу excel с расписанием и формирует необходимые данные для работы приложения",
                Command = main.LoadExcelCommand
            };

            var loadTempFile = new ToolViewModel()
            {
                Title = "Загрузить временный файл",
                Description = "Загружает временный файл для для просмотра",
                Command = main.LoadTempFileCommand
            };

            var loadCalculation = new ToolViewModel()
            {
                Title = "Загрузить расчёт",
                Description = "Загружает в сессию расчёт",
                Command = main.LoadCalculationCommand
            };

            var extractTempFile = new ToolViewModel()
            {
                Title = "Выгрузить во временный файл",
                Description = "Выгружает таблицу excel во временный файл таблицу",
                Command = main.ExtractToTempFileCommand
            };

            var exctractWorkload = new ToolViewModel()
            {
                Title = "Выдать загруженность",
                Description = "Формирует таблицу загруженности",
                Command = main.ExctractWorkloadTableCommand
            };

            var exctractReport = new ToolViewModel()
            {
                Title = "Выдать отчёт",
                Description = "Формирует таблицу-отчёт по преподавателям",
                Command = main.ExctractReportTableCommand
            };

            var fillIndividualPlan = new ToolViewModel()
            {
                Title = "Заполнить инд. план",
                Description = "Получает на вход файл word с индивидуальным планом и выдаёт его версию с заполненной таблицей",
                Command = main.FillIndividualPlanCommand
            };

            tools.Add(new ToolCategoryViewModel(loadSession, loadExcelSchedule, loadTempFile, loadCalculation)
            {
                Name = "Временное"
            });
        }
    }
}
