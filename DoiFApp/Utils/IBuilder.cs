namespace DoiFApp.Utils
{
    public interface IBuilder< T> where T : class
    {
        T Build();
    }
}
