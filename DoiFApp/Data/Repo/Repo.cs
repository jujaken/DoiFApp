using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DoiFApp.Data.Repo
{
    public class Repo<T>(AppDbContext context) : IRepo<T> where T : class
    {
        public AppDbContext Db { get; } = context;
        public DbSet<T> Set => Db.Set<T>();

        private IQueryable<T> query = context.Set<T>();

        public async Task Create(T model)
        {
            await Set.AddAsync(model);
            await Db.SaveChangesAsync();
        }

        public Task<List<T>> GetAll()
            => Task.FromResult(query.AsEnumerable().ToList());

        public Task<List<T>> GetWhere(Func<T, bool> condition)
            => Task.FromResult(query.Where(condition).ToList());

        public async Task Update(T model)
        {
            Set.Update(model);
            await Db.SaveChangesAsync();
        }

        public async Task Delete(T model)
        {
            Set.Remove(model);
            await Db.SaveChangesAsync();
        }

        public IRepo<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            query = query.Include(navigationPropertyPath);
            return this;
        }
    }
}
