using DoiFApp.Data.Models;
using Xceed.Document.NET;

namespace DoiFApp.Services.IndividualPlan
{
    public class PlanFirstHalfIndividualPlanDataWriter : AbstractIndividualPlanWriter<PlanFirstHalfIndividualPlanData>
    {
        protected override async Task UpdateTables(EducationTeacherModel teacher, List<Table> tables)
        {
            await InsertData(tables[0], teacher.PlanWorks1);
        }
    }
}
