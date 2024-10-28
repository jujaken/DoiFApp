using DoiFApp.Enums;
using DoiFApp.Utils.Attributes;

namespace DoiFApp.Utils.Extensions
{
    public static class NonEducationWorkTypeExtensions
    {
        public static string GetViewName(this NonEducationWorkType workType)
            => (workType.GetType()
                .GetMember(workType.ToString())[0]
                .GetCustomAttributes(typeof(ViewNameAttribute), inherit: false)[0]
                    as ViewNameAttribute)!.ViewName;

        public static int GetFirstId(this NonEducationWorkType workType)
            => (workType.GetType()
                .GetMember(workType.ToString())[0]
                .GetCustomAttributes(typeof(IPTableIdAttribute), inherit: false)[0]
                    as IPTableIdAttribute)!.FirstId;

        public static int GetSecondId(this NonEducationWorkType workType)
            => (workType.GetType()
                .GetMember(workType.ToString())[0]
                .GetCustomAttributes(typeof(IPTableIdAttribute), inherit: false)[0]
                    as IPTableIdAttribute)!.SecondId;
    }
}
