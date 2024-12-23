namespace DoiFApp.Config
{
    public sealed class ConfigColorCategory
    {
        public required string Tittle { get; set; }
        public List<ConfigColor> Colors { get; set; } = [];
    }
}
