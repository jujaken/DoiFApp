using DoiFApp.Services.Data;

namespace DoiFApp.Services.NonEducationWork
{
    public class NonEducationWorkData : IData
    {
        public IEnumerable<DoiFApp.Data.Models.NonEducationWork>? NonEducationWorks { get; init; }

        public bool IsHolistic => NonEducationWorks != null && NonEducationWorks.Any();

        public IEnumerable<object> AllObjects => NonEducationWorks!;
    }
}
