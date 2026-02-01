using GymManagementBLL.ViewModels.SessionViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int sessionId);
        bool CreateSession(SessionCreateViewModel sessionCreateViewModel);
        bool UpdateSession(int sessionId, SessionUpdateViewModel sessionUpdateViewModel);
        SessionUpdateViewModel? GetSessionToUpdate(int sessionId);
        bool RemoveSession(int sessionId);

    }
}
