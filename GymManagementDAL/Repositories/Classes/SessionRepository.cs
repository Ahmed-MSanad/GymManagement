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
        public IEnumerable<Session> GetAllSessionsWithCategoryAndTrainer() => 
            gymDbContext.Sessions.Include(s => s.Trainer).Include(s => s.Category).ToList();
        public int GetFreeSessionSlots(int sessionId)
        {
            var session = gymDbContext.Sessions.Where(s => s.Id == sessionId).FirstOrDefault();
            if (session is null) return 0;
            
            return session.Capacity - session.SessionMembers.Count();
        }

        public Session GetSessionWithCategoryAndTrainer(int sessionId)
            => gymDbContext.Sessions.Include(s => s.Trainer).Include(s => s.Category).FirstOrDefault(s => s.Id == sessionId);
    }
}
