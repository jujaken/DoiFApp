using DoiFApp.Data.Models;
using Xceed.Document.NET;

namespace DoiFApp.Services.IndividualPlan
{
    public class SecondHalfIndividualPlanDataWriter : AbstractIndividualPlanWriter<SecondHalfIndividualPlanData>
    {
        protected override void UpdateTables(EducationTeacherModel teacher, List<Table> tables)
        {
            // ПЛАНИРУЕМАЯ НА 1 ПОЛУГОДИЕ
            var w1Dones = InsertData(tables[0], teacher.PlanWorks1);
            // ПЛАНИРУЕМАЯ НА 2 ПОЛУГОДИЕ
            InsertData(tables[1], teacher.PlanWorks2, w1Dones);
        }
    }
}
