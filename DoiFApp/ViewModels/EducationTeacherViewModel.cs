﻿using CommunityToolkit.Mvvm.ComponentModel;
using DoiFApp.Data.Models;

namespace DoiFApp.ViewModels
{
    public class EducationTeacherViewModel() : ObservableObject
    {
        private EducationTeacherModel? model;

        public string Id => model is not null ? $"{model.Id}" : "-1";
        public string Name => model is not null ? $"{model.Name}" : "Name";
        public string Works1 => model?.Works1.Count.ToString() ?? "?";
        public string Works2 => model?.Works2.Count.ToString() ?? "?";
        public string ReallyWorks1 => model?.ReallyWorks1.Count.ToString() ?? "?";
        public string ReallyWorks2 => model?.ReallyWorks2.Count.ToString() ?? "?";

        public EducationTeacherViewModel(EducationTeacherModel model) : this()
                => SetModel(model);

        public void SetModel(EducationTeacherModel model)
        {
            this.model = model;
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Works1));
            OnPropertyChanged(nameof(Works2));
            OnPropertyChanged(nameof(ReallyWorks1));
            OnPropertyChanged(nameof(ReallyWorks2));
        }
    }
}