using DoiFApp.Data.Models;

namespace DoiFApp.Utils
{
    public static class DataUtil
    {
        public static List<string> GetTeachers(List<LessonModel> data)
        {
            var teachersNotUnique = data.Select(l => l.Teachers);
            var teachersUnique = new List<string>();

            foreach (var teachers in teachersNotUnique)
                foreach (var teacher in teachers)
                    if (!teachersUnique.Contains(teacher))
                        teachersUnique.Add(teacher);
            teachersUnique.Sort();
            return teachersUnique;
        }

    }
}
