using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Data.Repo;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels.Pages
{
    public partial class FillIndividualPlanPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? input;

        [ObservableProperty]
        private ObservableCollection<string> teachers = [];

        [ObservableProperty]
        private string? selectedTeacher;

        [RelayCommand]
        public async Task Update()
        {
            var repo = Ioc.Default.GetRequiredService<IRepo<EducationTeacherViewModel>>();
            var teachers = await (Input == null ? repo.GetAll() : repo.GetWhere(t => t.Name.Contains(Input)));
            Teachers.Clear();
            foreach(var teacher in teachers)
                Teachers.Add(teacher.Name);
        }

        public Action<EducationTeacherViewModel>? OnSelect { get; set; }

        [RelayCommand]
        public async Task Select()
        {
            var teacher = (await Ioc.Default.GetRequiredService<IRepo<EducationTeacherViewModel>>()
                .GetWhere(t => t.Name == SelectedTeacher!)).FirstOrDefault()
                    ?? throw new Exception("teacher not found");

            OnSelect?.Invoke(teacher);
        }
    }
}
