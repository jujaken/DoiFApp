namespace DoiFApp.Services
{
    public interface IDbCopier
    {
        Task Copy(string source, string target);
    }
}
