using DoiFApp;
using Microsoft.EntityFrameworkCore;

namespace EFTest.Data
{
    public class Repo<T>(AppDbContext context) : IRepo<T> where T : class
    {
        private readonly AppDbContext context = context;
        protected DbSet<T> Set => context.Set<T>();

        public async Task Create(T model)
        {
            await Set.AddAsync(model);
            await context.SaveChangesAsync();
        }

        public Task<List<T>> GetAll()
            => Task.FromResult(Set.AsEnumerable().ToList());

        public Task<List<T>> GetWhere(Func<T, bool> condition)
            => Task.FromResult(Set.Where(condition).ToList());

        public async Task<T?> GetById(int id)
            => await Set.FindAsync(id);

        public async Task Update(T model)
        {
            Set.Update(model);
            await context.SaveChangesAsync();
        }

        public async Task Delete(T model)
        {
            Set.Remove(model);
            await context.SaveChangesAsync();
        }
    }
}
