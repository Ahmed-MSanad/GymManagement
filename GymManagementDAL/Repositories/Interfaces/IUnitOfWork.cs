using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> GetRepository<T>() where T : BaseEntity;
        ISessionRepository SessionRepository { get; set; }
        IMemberShipRepository MemberShipRepository { get; set; }
        IBookingRepository BookingRepository { get; set; }
        int SaveChanges();
    }
}
