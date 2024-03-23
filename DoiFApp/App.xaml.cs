using CommunityToolkit.Mvvm.DependencyInjection;
using DoiFApp.Data;
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services;
using DoiFApp.Services.Excel;
using DoiFApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
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

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddDbContext<AppDbContext>()
                // view models:
                .AddTransient<NotifyViewModel>()
                .AddTransient<ToolViewModel>()
                // services:
                .AddTransient<IDataReader, ExcelReader>()
                .AddTransient<ITempFileWorker, ExcelTempFileWorker>()
                .AddTransient<IRepo<LessonModel>, Repo<LessonModel>>() 
                .AddTransient<NotifyBuilder>()
                .BuildServiceProvider();
        }
    }
}
