using DoiFApp.Data.Models;

namespace DoiFApp.Services
{
    public interface IEducationReader
    {
        Task ReadFromFile(string fileName);
    }
}
