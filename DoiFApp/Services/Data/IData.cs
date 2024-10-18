namespace DoiFApp.Services.Data
{
    public interface IData
    {
        public bool IsHolistic { get; }
        public IEnumerable<object> AllObjects { get; }
    }
}
