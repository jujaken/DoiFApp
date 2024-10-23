using DoiFApp.Data.Repo;
using DoiFApp.Services.Data;

namespace DoiFApp.Services.NonEducationWork
{
    public class SessionNonEducationWorkDataSaver(IRepo<DoiFApp.Data.Models.NonEducationWork> repo)
        : AbstractSessionDataSaver<DoiFApp.Data.Models.NonEducationWork, NonEducationWorkData>(repo)
    {
    }
}
