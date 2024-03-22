namespace DoiFApp.Services
{
    public interface ITempFileWorker
    {
        Task WriteFile(string path);
        Task ReadFile(string path);
    }
}
