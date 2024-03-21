namespace DoiFApp.Repo
{
    public interface IRepo<T> where T : class
    {
        Task Create(T model);
        Task<List<T>> GetAll();
        Task<List<T>> GetWhere(Func<T, bool> condition);
        Task<T?> GetById(int id);
        Task Update(T model);
        Task Delete(T model);
    }
}
