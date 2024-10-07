namespace DoiFApp.Services.Data
{
    public interface IDataWriter<T> where T : IData
    {
        Task<bool> Write(T data, string path);
    }
}
