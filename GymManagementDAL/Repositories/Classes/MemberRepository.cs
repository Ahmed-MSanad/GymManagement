using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        private readonly GymDbContext context;
        public MemberRepository(GymDbContext _context) : base(_context) { 
            context = _context;
        }
        public IEnumerable<Session> GetAllSessions(int memberId)
        {
            return (IEnumerable<Session>)context.Members.Where(m => m.Id == memberId).Include(m => m.MemberSessions).ToList();
        }
    }
}
