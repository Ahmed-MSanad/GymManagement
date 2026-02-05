using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext gymDbContext;
        public SessionRepository(GymDbContext _gymDbContext) : base(_gymDbContext) {
            gymDbContext = _gymDbContext;
        }
        public IEnumerable<Session> GetAllSessionsWithCategoryAndTrainer()
        {
            var sessions = gymDbContext.Sessions.Include(s => s.Trainer).Include(s => s.Category).ToList();
            return sessions;
        }
        public int GetFreeSessionSlots(int sessionId)
        {
            var session = gymDbContext.Sessions.Where(s => s.Id == sessionId).Include(s => s.SessionMembers).FirstOrDefault();
            if (session is null) return 0;

            var free = session.Capacity - session.SessionMembers.Count();

            return free;
        }

        public Session GetSessionWithCategoryAndTrainer(int sessionId)
            => gymDbContext.Sessions.Include(s => s.Trainer).Include(s => s.Category).FirstOrDefault(s => s.Id == sessionId);
    }
}
