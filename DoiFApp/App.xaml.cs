using CommunityToolkit.Mvvm.DependencyInjection;
using DoiFApp.Data;
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services;
using DoiFApp.Services.Excel;
using DoiFApp.Services.Word;
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

        private static ServiceProvider ConfigureServices() => new ServiceCollection()
                .AddDbContext<AppDbContext>()
                // view models:
                .AddTransient<NotifyViewModel>()
                .AddTransient<ToolViewModel>()
                // services:
                .AddTransient<IDataReader, ExcelReader>()
                .AddTransient<ITempFileWorker, ExcelTempFileWorker>()
                .AddTransient<IReportWriter, ExcelReportWriter>()
                .AddTransient<IWorkSchedule, ExcelWorkSchedule>()
                .AddTransient<IEducationReader, ExcelEducationReader>()
                .AddTransient<IIndividualPlanWriter, WordIndividualPlanWriter>()
                .AddTransient<ITeacherFinder, TeacherFinder>()
                .AddTransient<IRepo<LessonModel>, Repo<LessonModel>>()
                .AddTransient<IRepo<EducationTeacherModel>, Repo<EducationTeacherModel>>()
                .AddTransient<IRepo<EducationWorkModel>, Repo<EducationWorkModel>>()
                .AddTransient<NotifyBuilder>()
                .BuildServiceProvider();
    }
}
