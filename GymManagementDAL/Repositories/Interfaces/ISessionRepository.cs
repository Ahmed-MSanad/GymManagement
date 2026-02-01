using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithCategoryAndTrainer();
        int GetFreeSessionSlots(int sessionId);
        Session GetSessionWithCategoryAndTrainer(int sessionId);
    }
}
