using DoiFApp.Data.Models;
using DoiFApp.Services.Data;

namespace DoiFApp.Services.IndividualPlan
{
    public abstract class AbstractIndividualPlanData : IData
    {
        public EducationTeacherModel? TeacherModel { get; init; }
        public bool IsHolistic => TeacherModel != null;
    }
}
