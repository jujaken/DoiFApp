using DoiFApp.Data.Models;
using Xceed.Document.NET;

namespace DoiFApp.Services.IndividualPlan
{
    public class WordFactSecondHalfIndividualPlanDataWriter : WordAbstractIndividualPlanWriter<FactSecondHalfIndividualPlanData>
    {
        protected override async Task UpdateTables(EducationTeacherModel teacher, List<Table> tables)
        {
            var table = tables[3];
            await InsertData(table, teacher.PlanWorks2, true);
            await InsertDones(table, tables[2]);
        }
    }
}
