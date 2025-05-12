using CommunityToolkit.Mvvm.DependencyInjection;
using DoiFApp.Data;
using DoiFApp.Data.Models;
using DoiFApp.Data.Repo;
using DoiFApp.Services;
using DoiFApp.Services.Builders;
using DoiFApp.Services.Data;
using DoiFApp.Services.Education;
using DoiFApp.Services.IndividualPlan;
using DoiFApp.Services.MonthlyIndividualPlan;
using DoiFApp.Services.NonEducationWork;
using DoiFApp.Services.Schedule;
using DoiFApp.Services.TempSchedule;
using DoiFApp.Services.Workload;
using DoiFApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using System.Windows;

namespace DoiFApp
{
    public partial class App : Application
    {
        public const string DbPath = "doifapp.db";
        public const string SettingsPath = "doif-colors.json";

        public IServiceProvider Services { get; }

        public App()
        {
            Services = ConfigureServices();
            Ioc.Default.ConfigureServices(Services);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private static ServiceProvider ConfigureServices()
            => new ServiceCollection()
                .AddDbContext<AppDbContext>()
                // view models:
                .AddTransient<NotifyViewModel>()
                .AddTransient<ToolViewModel>()
                .AddTransient<ToolCategoryViewModel>()
                // repos:
                .AddTransient<IRepo<LessonModel>, Repo<LessonModel>>()
                .AddTransient<IRepo<EducationTeacherModel>, Repo<EducationTeacherModel>>()
                .AddTransient<IRepo<EducationWorkModel>, Repo<EducationWorkModel>>()
                .AddTransient<IRepo<EducationTypeAndHourModel>, Repo<EducationTypeAndHourModel>>()
                .AddTransient<IRepo<LessonTypeConverter>, Repo<LessonTypeConverter>>()
                // builders
                .AddTransient<NotifyBuilder>()
                // education
                .AddTransient<IDataReader<PlanEducationData>, ExcelPlanEducationDataReader>()
                .AddTransient<IDataSaver<PlanEducationData>, SessionPlanEducationDataSaver>()
                .AddTransient<IDataReader<FactEducationData>, ExcelFactEducationDataReader>()
                .AddTransient<IDataSaver<FactEducationData>, SessionFactEducationDataSaver>()
                // individual plan
                .AddTransient<IDataWriter<PlanFirstHalfIndividualPlanData>, WordPlanFirstHalfIndividualPlanDataWriter>()
                .AddTransient<IDataWriter<PlanSecondHalfIndividualPlanData>, WordPlanSecondHalfIndividualPlanDataWriter>()
                .AddTransient<IDataWriter<FactFirstHalfIndividualPlanData>, WordFactFirstHalfIndividualPlanDataWriter>()
                .AddTransient<IDataWriter<FactSecondHalfIndividualPlanData>, WordFactSecondHalfIndividualPlanDataWriter>()
                .AddTransient<IDataWriter<MonthlyIndividualPlanData>, WordMonthlyIndividualPlanDataWriter>()
                // non education
                .AddTransient<IDataReader<NonEducationWorkData>, WordNonEducationWorkDataReader>()
                .AddTransient<IDataWriter<NonEducationWorkData>, IndividualPlanNonEducationWorkDataWriter>()
                // schedule
                .AddTransient<IDataReader<ScheduleData>, ExcelScheduleReader>()
                .AddTransient<IDataWriter<ScheduleData>, ExcelScheduleWriter>()
                .AddTransient<IDataSaver<ScheduleData>, SessionScheduleSaver>()
                // temp schedule
                .AddTransient<IDataReader<TempScheduleData>, ExcelTempScheduleReader>()
                .AddTransient<IDataWriter<TempScheduleData>, ExcelTempScheduleWriter>()
                .AddTransient<IDataSaver<TempScheduleData>, ExcelTempScheduleSaver>()
                // workload
                .AddTransient<IDataWriter<WorkloadData>, ExcelWorkloadWriter>()
                // other services
                .AddTransient<ITeacherFinder, TeacherFinder>()
                .AddTransient<IDbCopier, SqliteDbCopy>()
                .AddTransient<IAppConfigService, JsonAppConfigService>()
                .BuildServiceProvider();
    }
}
