using System.ComponentModel;

namespace DoiFApp.ViewModels.Pages
{
    public partial class FillMonthlyIndividualPlanPageViewModel : FillIndividualPlanPageViewModel
    {
        protected override void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            base.OnPropertyChanging(e);

            if (e.PropertyName == nameof(IsFirstSemester) && IsSecondSemester)
            {
                IsSecondSemester = false;
            }

            if (e.PropertyName == nameof(IsSecondSemester) && IsFirstSemester)
            {
                IsFirstSemester = false;
            }
        }
    }
}
