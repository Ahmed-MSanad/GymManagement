using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        IEnumerable<Session> GetAllSessions(int memberId);
    }
}
