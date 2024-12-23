using DoiFApp.Config;

namespace DoiFApp.Services
{
    public interface IAppConfigService
    {
        Task Save(AppConfig appConfig, string path);
        Task Copy(AppConfig appConfig, string path);
        Task<AppConfig?> Get(string path);
        Task<AppConfig> SetDefault(string path);
    }
}