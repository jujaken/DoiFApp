namespace DoiFApp.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class ViewNameAttribute(string viewName) : Attribute
    {
        public string ViewName { get; } = viewName;
    }
}
