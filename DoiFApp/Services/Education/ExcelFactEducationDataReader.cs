using DoiFApp.Enums;

namespace DoiFApp.Services.Education
{
    public class ExcelFactEducationDataReader : AbstractExcelEducationReader<FactEducationData>
    {
        public override async Task<FactEducationData> Read(string path)
        {
            var data = await Read(path, "отч", WorkCategory.FactFirstSemester, WorkCategory.FactSecondSemester);
            return new()
            {
                TeacherModels = data.TeacherModels,
                TypeAndHourModels = data.TypeAndHourModels,
                WorkModels = data.WorkModels,
            };
        }
    }
}
