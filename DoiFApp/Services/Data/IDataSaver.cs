namespace DoiFApp.Services.Data
{
    public interface IDataSaver<T> where T : IData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>получилось ли сохранить данные</returns>
        Task<bool> Save(T data);
    }
}
