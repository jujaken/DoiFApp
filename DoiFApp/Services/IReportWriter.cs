namespace DoiFApp.Services
{
    public interface IReportWriter
    {
        Task Write(string path);
    }
}
