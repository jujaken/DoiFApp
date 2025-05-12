namespace DoiFApp.Config
{
    public sealed class ConfigColor
    {
        public required string Key { get; set; }

        /// <summary> List для более качественной сериализации в [255, 0, 0] формат </summary>
        public List<byte> Value { get; set; } = [0, 0, 0];
    }
}
