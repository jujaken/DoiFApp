﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using System.Text;

namespace DoiFApp.ViewModels
{
    public class LessonViewModel() : ObservableObject
    {
        private LessonModel? model;

        public string Id => model is not null ? $"{model.Id}" : "-1";
        public string Date => model is not null ? $"{model.Date:dd.MM.yyyy}" : "00.00.0000";
        public string Time => model?.Time ?? "00:00-00:00";
        public string Discipline => model?.Discipline ?? "math";
        public string LessionType => model?.LessionType ?? "s";
        public string Topic => model?.Topic ?? "-";
        public string Teachers => model is not null ? GetListStr(model.Teachers, '\n') : "Ivanov I.I.";
        public string Groups => model is not null ? GetListStr(model.Groups, ',') : "666";
        public string Auditoriums => model is not null ? GetListStr(model.Auditoriums, ',') : "777";

        public double Wight
        {
            get => model?.Wight ?? 0;
            set
            {
                if (model is null || model.Wight == value)
                    return;

                SetProperty(model.Wight, value, model, (m, v) => m.Wight = v);
                Ioc.Default.GetRequiredService<IRepo<LessonModel>>().Update(model);
            }
        }

        public LessonViewModel(LessonModel model) : this()
            => SetModel(model);

        public void SetModel(LessonModel model)
        {
            this.model = model;
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(Time));
            OnPropertyChanged(nameof(Discipline));
            OnPropertyChanged(nameof(LessionType));
            OnPropertyChanged(nameof(Teachers));
            OnPropertyChanged(nameof(Groups));
            OnPropertyChanged(nameof(Auditoriums));
        }

        private string GetListStr(List<string> items, char v)
        {
            var strBuilder = new StringBuilder();

            for (int i = 0; i < items.Count - 1; i++)
                strBuilder.Append(items[i] + v);
            strBuilder.Append(items[^1]);

            return strBuilder.ToString();
        }
    }
}