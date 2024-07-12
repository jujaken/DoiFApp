using DoiFApp.Data.Models;

namespace DoiFApp.Services
{
    public interface IIndividualPlanWriter
    {
        Task MakePlans(string path);
    }
}
