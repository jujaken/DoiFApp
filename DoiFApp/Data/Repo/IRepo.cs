using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DoiFApp.Data.Repo
{
    public interface IRepo<T> where T : class
    {
        AppDbContext Db { get; }
        DbSet<T> Set { get; }
        Task Create(T model);
        Task<List<T>> GetAll();
        Task<List<T>> GetWhere(Func<T, bool> condition);
        Task Update(T model);
        Task Delete(T model);
        IRepo<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);
    }
}
