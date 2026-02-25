using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class MemberShipRepository : GenericRepository<MemberShip>, IMemberShipRepository
    {
        private readonly GymDbContext context;
        public MemberShipRepository(GymDbContext _context) : base(_context)
        {
            context = _context;
        }
        public IEnumerable<MemberShip>? GetAllMemberShipsIncludingPlansAndMembers(Func<MemberShip, bool>? condition = null)
        {
            if (context.MemberShips is not null && context.MemberShips.Any())
            {
                if(condition is not null)
                    return context.MemberShips.Include(ms => ms.Plan).Include(ms => ms.Member).Where(condition).ToList();
                else
                    return context.MemberShips.Include(ms => ms.Plan).Include(ms => ms.Member).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}
