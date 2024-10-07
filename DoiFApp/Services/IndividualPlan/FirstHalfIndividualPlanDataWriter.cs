using DoiFApp.Data.Models;
using Xceed.Document.NET;

namespace DoiFApp.Services.IndividualPlan
{
    public class FirstHalfIndividualPlanDataWriter : AbstractIndividualPlanWriter<FirstHalfIndividualPlanData>
    {
        protected override void UpdateTables(EducationTeacherModel teacher, List<Table> tables)
        {
            // ПЛАНИРУЕМАЯ НА 1 ПОЛУГОДИЕ
            var w1Dones = InsertData(tables[0], teacher.Works1);
        }
    }
}
