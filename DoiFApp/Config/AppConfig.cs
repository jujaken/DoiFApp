using DoiFApp.Utils;

namespace DoiFApp.Config
{
    public class AppConfig
    {
        public List<ConfigColorCategory> ConfigColorCategories { get; set; } = [];

        public static AppConfig DefaultConfig => new()
        {
            ConfigColorCategories =
            [
                new ConfigColorCategory() {
                    Tittle = WorkloadHelper.CategoryName,
                    Colors =
                    [
                        new ConfigColor() { Key = WorkloadHelper.SaturdayColorName, Value = WorkloadHelper.SaturdayColorDefault },
                        new ConfigColor() { Key = WorkloadHelper.SundayColorName, Value = WorkloadHelper.SundayColorDefault },

                        new ConfigColor() { Key = WorkloadHelper.KoptevoColorName, Value = WorkloadHelper.KoptevoColorDefault },
                        new ConfigColor() { Key = WorkloadHelper.VolginoColorName, Value = WorkloadHelper.VolginoColorDefault },

                        new ConfigColor() { Key = WorkloadHelper.BobruiskayaColorName, Value = WorkloadHelper.OtherColorDefault },
                        new ConfigColor() { Key = WorkloadHelper.GlavnayaColorName, Value = WorkloadHelper.OtherColorDefault },
                        new ConfigColor() { Key = WorkloadHelper.OkrujnoyColorName, Value = WorkloadHelper.OtherColorDefault },
                        new ConfigColor() { Key = WorkloadHelper.KolskayaColorName, Value = WorkloadHelper.OtherColorDefault },
                        new ConfigColor() { Key = WorkloadHelper.DmitrovkaColorName, Value = WorkloadHelper.OtherColorDefault },
                        new ConfigColor() { Key = WorkloadHelper.PhilimonkovskoyeColorName, Value = WorkloadHelper.OtherColorDefault },
                        new ConfigColor() { Key = WorkloadHelper.OtherColorName, Value = WorkloadHelper.OtherColorDefault },

                        new ConfigColor() { Key = WorkloadHelper.TransitionColorName, Value = WorkloadHelper.TransitionColorDefault },
                        new ConfigColor() { Key = WorkloadHelper.WithoutColorName, Value = WorkloadHelper.WithoutColorDefault },
                    ]
                 },
            ],
        };
    }
}
