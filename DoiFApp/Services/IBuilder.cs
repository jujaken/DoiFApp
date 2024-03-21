namespace DoiFApp.Services
{
    public interface IBuilder<T> where T : class
    {
        T Build();
    }
}
