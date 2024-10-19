using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using System.Collections.ObjectModel;
using System.Windows.Media;

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

            LessonTypeTranslations = [.. uniqueTypes.Select(t => new LessonTypeTranslateViewModel() { CurrentName = t, NewName = t })];
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

        [RelayCommand]
        public async Task Ok()
        {
            if (OnOk != null)
                await OnOk.Invoke(LessonTypeTranslations.AsEnumerable());
        }
    }
}
