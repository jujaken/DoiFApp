using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels.Pages
{
    public partial class DataPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<LessonViewModel> lessonViewModels = [];

        [ObservableProperty]
        private ObservableCollection<EducationTeacherViewModel> educationTeacherModel = [];

        [RelayCommand]
        public async Task LoadData()
        {
            LessonViewModels = new((await Ioc.Default.GetRequiredService<IRepo<LessonModel>>().GetAll())
                .Select(l => new LessonViewModel(l)));

            EducationTeacherModel = new((await Ioc.Default.GetRequiredService<IRepo<EducationTeacherModel>>()
                    .Include(at => at.Works).GetAll())
                .Select(l => new EducationTeacherViewModel(l)));
        }
    }
}
