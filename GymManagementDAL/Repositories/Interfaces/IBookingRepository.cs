using GymManagementDAL.Entities;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        IEnumerable<Booking>? GetBookingsWithDetails(Func<Booking, bool>? condition = null);
    }
}
