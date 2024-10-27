using DoiFApp.Enums;
using System.ComponentModel;

namespace DoiFApp.Utils.Extensions
{
    public static class NonEducationWorkTypeExtensions
    {
        public static string GetDescription(this NonEducationWorkType workType)
            => (workType.GetType()
                .GetMember(workType.ToString())[0]
                .GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)[0]
                as DescriptionAttribute)!.Description;
    }
}
