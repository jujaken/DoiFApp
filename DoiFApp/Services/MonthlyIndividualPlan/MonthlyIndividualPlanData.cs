using DoiFApp.Data.Models;
using DoiFApp.Services.IndividualPlan;

namespace DoiFApp.Services.MonthlyIndividualPlan
{
    public class MonthlyIndividualPlanData : AbstractIndividualPlanData
    {
        public IEnumerable<LessonModel>? Lessons { get; init; }
        public override IEnumerable<object> AllObjects => base.AllObjects.Union(Lessons!.Cast<object>());
    }
}
