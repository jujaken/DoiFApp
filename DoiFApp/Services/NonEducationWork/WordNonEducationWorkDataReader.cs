using DoiFApp.Services.Data;
using Xceed.Words.NET;

namespace DoiFApp.Services.NonEducationWork
{
    public class WordNonEducationWorkDataReader : IDataReader<NonEducationWorkData>
    {
        public Task<NonEducationWorkData> Read(string path)
        {
            using var doc = DocX.Load(path);

            var educationWork = new List<DoiFApp.Data.Models.NonEducationWork>();

            var prefix = string.Empty;
            var lastIsPrefix = false;
            var lastPrefixIsMetodic = doc.Paragraphs[0].Text.Contains("2.1. Разработка и переработка учебно-методических материалов");
            var paragraths = lastPrefixIsMetodic ? doc.Paragraphs.Skip(1) : doc.Paragraphs;
            foreach (var paragrath in paragraths)
            {
                var text = paragrath.Text;
                if (string.IsNullOrEmpty(text)) continue;
                if (int.TryParse(text[0].ToString(), out var num))
                {
                    if (lastIsPrefix)
                        educationWork.Add(new() { Text = prefix.Trim(':', ' ') });

                    prefix = string.Empty;
                    foreach (var part in text.Split(' ')[1..])
                        prefix += part + ' ';
                    prefix = prefix.Trim('.', ' ', ':', ';');
                    prefix += ": ";

                    lastIsPrefix = true;
                    lastPrefixIsMetodic = false;
                    continue;
                }
                else
                    lastIsPrefix = false;

                if (lastPrefixIsMetodic)
                {
                    educationWork.Add(new() { Text = "Разработка " + text[0].ToString().ToLower() + text[1..] });
                    educationWork.Add(new() { Text = "Переработка " + text[0].ToString().ToLower() + text[1..] });
                    continue;
                }

                educationWork.Add(new() { Text = prefix + text[0].ToString().ToLower() + text[1 ..] });
            }
            return Task.FromResult(new NonEducationWorkData() { NonEducationWorks = educationWork });
        }
    }
}
