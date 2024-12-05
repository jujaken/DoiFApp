using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using System.Collections.ObjectModel;
using System.Windows;

namespace DoiFApp.ViewModels.Pages
{
    public partial class CheckSchedulePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<LessonTypeTranslateViewModel> lessonTypeTranslations = [
                new LessonTypeTranslateViewModel() { CurrentName = "Лекция", NewName = "Лекция"}
            ];

        public async Task Update()
        {
            var uniqueTypes = new List<string>();

            (await Ioc.Default.GetRequiredService<IRepo<LessonModel>>().GetAll()).ForEach(l =>
            {
                if (!uniqueTypes.Contains(l.LessionType))
                    uniqueTypes.Add(l.LessionType);
            });

            LessonTypeTranslations.Clear();

            foreach (var type in uniqueTypes)
                LessonTypeTranslations.Add(new LessonTypeTranslateViewModel()
                {
                    CurrentName = type,
                    NewName = type,
                    SelectedConvertion = (await Ioc.Default.GetRequiredService<IRepo<LessonTypeConverter>>()
                        .GetWhere(c => c.TypeName == type)).FirstOrDefault()?.Convertion,
                });
        }

        [RelayCommand]
        public void Return()
        {
            foreach (var type in LessonTypeTranslations)
                type.NewName = type.CurrentName;
        }

        public event Action? OnCancel;

        [RelayCommand]
        public void Cancel()
        {
            OnCancel?.Invoke();
        }

        public event Func<IEnumerable<LessonTypeTranslateViewModel>, Task>? OnOk;
        public event Func<Task>? OnError;

        [RelayCommand]
        public async Task Ok()
        {
            if (LessonTypeTranslations.All(t => t.SelectedConvertion != null))
            {
                if (OnOk != null)
                    await OnOk.Invoke(LessonTypeTranslations.AsEnumerable());
                return;
            }
            if (OnError != null)
                await OnError.Invoke();
        }
    }
}
