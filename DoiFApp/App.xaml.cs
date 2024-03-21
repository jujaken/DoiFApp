using CommunityToolkit.Mvvm.DependencyInjection;
using DoiFApp.Services;
using DoiFApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace DoiFApp
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();
            Ioc.Default.ConfigureServices(Services);
        }

        private IServiceProvider ConfigureServices()
        {
            var sc = new ServiceCollection()
                // view models:
                .AddTransient<NotifyViewModel>()
                .AddTransient<ToolViewModel>()
                // services:
                .AddTransient<IExcelReader, ExcelReader>()
                .AddTransient<NotifyBuilder>()
                /* so cool dependency injection*/;

            return sc.BuildServiceProvider();
        }
    }
}
