using CommunityToolkit.Mvvm.ComponentModel;
using DoiFApp.Data.Models;

namespace DoiFApp.ViewModels
{
    public partial class NonEducationWorkViewModel() : ObservableObject
    {
        public NonEducationWork? NonEducationWork { get; set; }

        public NonEducationWorkViewModel(NonEducationWork nonEducationWork) : this()
        {
            NonEducationWork = nonEducationWork;
        }

        public string Text => NonEducationWork?.Text ?? "None";

        [ObservableProperty]
        private bool isSelected;

        [ObservableProperty]
        private bool isFirstSemester;

        [ObservableProperty]
        private bool isSecondSemester;
    }
}
