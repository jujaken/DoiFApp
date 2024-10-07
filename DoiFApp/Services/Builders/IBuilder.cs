namespace DoiFApp.Services.Builders
{
    public interface IBuilder<T> where T : class
    {
        T Build();
    }
}
