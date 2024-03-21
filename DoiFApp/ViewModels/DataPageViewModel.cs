using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels
{
    public partial class DataPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<LessonViewModel> lessonViewModels = [];

        [RelayCommand]
        public async Task LoadLessonData()
        {
            LessonViewModels = new((await Ioc.Default.GetRequiredService<IRepo<LessonModel>>().GetAll())
                .Select(l => new LessonViewModel(l)));
        }
    }
}
