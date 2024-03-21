namespace DoiFApp.Services
{
    public interface IExcelReader
    {
        Task ReadToData(string path);
    }
}
