namespace DoiFApp.Services
{
    public interface IIndividualPlanWriter
    {
        Task FillPlan(string teacherName, string path);
        Task MakePlans(string path);
    }
}
