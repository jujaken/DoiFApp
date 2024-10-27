using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DoiFApp.Services;
using System.Collections.ObjectModel;

namespace DoiFApp.ViewModels.Pages
{
    public partial class FillIndividualPlanPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? input;

        [ObservableProperty]
        private ObservableCollection<string> teachers = [];

        private string? selectedTeacher;
        public string? SelectedTeacher
        {
            get => selectedTeacher;
            set
            {
                SetProperty(ref selectedTeacher, value);
                CanOk = SelectedTeacher != null;
            }
        }

        [ObservableProperty]
        private bool isFirstSemester = true;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OkCommand))]
        private bool canOk;

        [RelayCommand]
        public async Task Update()
        {
            var finder = Ioc.Default.GetRequiredService<ITeacherFinder>();
            var teachers = await finder.FindByPart(Input);
            Teachers.Clear();
            if (teachers != null)
                foreach (var teacher in teachers)
                    Teachers.Add(teacher.Name);
        }

        public event Action? OnCancel;

        [RelayCommand]
        public void Cancel()
        {
            OnCancel?.Invoke();
        }

        public event Func<(string teacherName, bool isFirstSemester), Task>? OnOk;

        [RelayCommand(CanExecute = nameof(CanOk))]
        public async Task Ok()
        {
            if (OnOk != null)
                await OnOk.Invoke((selectedTeacher!, IsFirstSemester));
        }
    }
}
