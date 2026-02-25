using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberShipRepository : IGenericRepository<MemberShip>
    {
        public IEnumerable<MemberShip>? GetAllMemberShipsIncludingPlansAndMembers(Func<MemberShip, bool>? condition = null);
    }
}
