﻿using DoiFApp.Config;
using System.IO;
using System.Text.Json;

namespace DoiFApp.Services
{
    public class JsonAppConfigService : IAppConfigService
    {
        private readonly static JsonSerializerOptions options = new()
        {
            WriteIndented = true,
        };

        public async Task Save(AppConfig appConfig, string path)
        {
            using var fs = File.OpenWrite(path);
            await JsonSerializer.SerializeAsync(fs, appConfig, options);
        }

        public async Task<AppConfig?> Get(string path)
        {
            if (!File.Exists(path))
                return null;

            using var fs = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<AppConfig?>(fs, options);
        }
        public Task Copy(AppConfig appConfig, string path)
          => Save(appConfig, path);

        public async Task<AppConfig> SetDefault(string path)
        {
            await Save(AppConfig.DefaultConfig, path);
            return AppConfig.DefaultConfig;
        }
    }
}
