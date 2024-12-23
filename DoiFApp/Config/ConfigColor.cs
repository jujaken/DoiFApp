using System.Drawing;

namespace DoiFApp.Config
{
    public sealed class ConfigColor
    {
        public required string Key { get; set; }
        public byte[] Value { get; set; } = [0, 0, 0];
    }
}
