namespace DoiFApp.Services.Schedule
{
    public interface IScheduleReader
    {
        Task ReadToData(string path);
    }
}
