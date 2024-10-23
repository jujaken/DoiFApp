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
            foreach (var paragrath in doc.Paragraphs)
            {
                var text = paragrath.Text;
                if (string.IsNullOrEmpty(text)) continue;
                educationWork.Add(new() { Text = text });
            }
            return Task.FromResult(new NonEducationWorkData() { NonEducationWorks = educationWork });
        }
    }
}
