using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;

namespace DoiFApp.Services.Data
{
    public abstract class AbstractSessionDataSaver<TModel, TData>(IRepo<TModel> repo)
        : IDataSaver<TData> where TModel : Model
                            where TData : IData
    {
        protected readonly IRepo<TModel> repo = repo;

        public virtual async Task<bool> Save(TData data)
        {
            if (data.IsHolistic)
            {
                await repo.Db.AddRangeAsync(data.AllObjects);
                await repo.Db.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
