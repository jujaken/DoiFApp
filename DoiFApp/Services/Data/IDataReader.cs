namespace DoiFApp.Services.Data
{
    public interface IDataReader<T> where T : IData
    {
        Task<T> Read(string path);
    }
}
