﻿using DoiFApp.ViewModels;
using System.Collections.ObjectModel;

namespace DoiFApp.Utils
{
    public static class ToolsHelper
    {
        // todo func all
        public static void AddDefaults(ObservableCollection<ToolCategoryViewModel> tools)
        {
            var loadSchedule = new ToolViewModel()
            {
                Title = "Загрузить расписание",
                Description = "Загружает таблицу excel с расписанием и формирует необходимые данные для работы приложения",
                Command = null
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

            tools.Add(new ToolCategoryViewModel("Индивидуальный план",
                fillIndividualPlan,
                loadCalculation,
                loadMethodicalWork,
                loadScientificWork,
                moralMentalWork,
                foreignersWork,
                otherWork
                ));

            // todo fact logic

            tools.Add(new ToolCategoryViewModel("Фактическая нагрузка"));

            var fromReport = new ToolViewModel()
            {
                Title = "Сформировать отчёт",
                Description = "Формулирует и выгружает данные для отчёта в excel",
                Command = null
            };

            tools.Add(new ToolCategoryViewModel("Отчётная документация",
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

            tools.Add(new ToolCategoryViewModel("Загруженность преподавателей",
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

            tools.Add(new ToolCategoryViewModel("DoiF",
                loadLastSession,
                exitSession,
                clearSession,
                importSession,
                exportSession
                ));

        }
    }
}
