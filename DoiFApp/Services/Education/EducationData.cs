using DoiFApp.Data.Models;
using DoiFApp.Services.Data;

namespace DoiFApp.Services.Education
{
    public class EducationData : IData
    {
        public IEnumerable<EducationTeacherModel>? TeacherModels { get; init; }
        public IEnumerable<EducationTypeAndHourModel>? TypeAndHourModels { get; init; }
        public IEnumerable<EducationWorkModel>? WorkModels { get; init; }

        public bool IsHolistic => TeacherModels != null && TeacherModels.Any()
            && TypeAndHourModels != null && TypeAndHourModels.Any()
            && WorkModels != null && WorkModels.Any();

        public IEnumerable<object> AllObjects => TeacherModels!.Cast<object>()
                                                 .Union(TypeAndHourModels!.Cast<object>())
                                                 .Union(WorkModels!.Cast<object>());
    }
}
