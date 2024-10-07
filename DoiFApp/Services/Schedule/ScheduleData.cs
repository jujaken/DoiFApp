using DoiFApp.Data.Models;
using DoiFApp.Services.Data;

namespace DoiFApp.Services.Schedule
{
    public class ScheduleData : IData
    {
        public IEnumerable<LessonModel>? Lessons { get; init; }
        public bool IsHolistic => Lessons != null;
    }
}
