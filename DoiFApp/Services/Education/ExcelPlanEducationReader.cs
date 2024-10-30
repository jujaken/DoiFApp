using DoiFApp.Enums;

namespace DoiFApp.Services.Education
{
    public class ExcelPlanEducationReader : AbstractExcelEducationReader<PlanEducationData>
    {
        public override async Task<PlanEducationData> Read(string path)
        {
            var data = await Read(path, "расч", WorkCategory.PlanFirstSemester, WorkCategory.PlanSecondSemester);
            return new()
            {
                TeacherModels = data.TeacherModels,
                TypeAndHourModels = data.TypeAndHourModels,
                WorkModels = data.WorkModels,
            };
        }
    }
}
