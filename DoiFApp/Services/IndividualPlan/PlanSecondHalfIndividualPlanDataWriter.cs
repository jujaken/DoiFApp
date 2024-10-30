using DoiFApp.Data.Models;
using Xceed.Document.NET;

namespace DoiFApp.Services.IndividualPlan
{
    public class PlanSecondHalfIndividualPlanDataWriter : AbstractIndividualPlanWriter<PlanSecondHalfIndividualPlanData>
    {
        protected override async Task UpdateTables(EducationTeacherModel teacher, List<Table> tables)
        {
            var table = tables[1];
            await InsertData(table, teacher.PlanWorks2);
            await InsertDones(table, tables[0]);
        }
    }
}
