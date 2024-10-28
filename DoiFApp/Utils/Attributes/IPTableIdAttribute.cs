namespace DoiFApp.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class IPTableIdAttribute(int firstId, int secondId) : Attribute
    {
        public int FirstId { get; } = firstId;
        public int SecondId { get; } = secondId;
    }
}
