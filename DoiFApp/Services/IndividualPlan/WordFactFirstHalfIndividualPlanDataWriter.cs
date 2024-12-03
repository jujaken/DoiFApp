using DoiFApp.Data.Models;
using Xceed.Document.NET;

namespace DoiFApp.Services.IndividualPlan
{
    public class WordFactFirstHalfIndividualPlanDataWriter : WordAbstractIndividualPlanWriter<FactFirstHalfIndividualPlanData>
    {
        protected override async Task UpdateTables(EducationTeacherModel teacher, List<Table> tables)
        {
            await InsertData(tables[2], teacher.FactWorks1);
        }
    }
}
