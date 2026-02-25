using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Repositories.Classes
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly GymDbContext context;
        public BookingRepository(GymDbContext _context) : base(_context)
        {
            context = _context;
        }
        public IEnumerable<Booking>? GetBookingsWithDetails(Func<Booking, bool>? condition = null)
        {
            if(context.Bookings is not null && context.Bookings.Any())
            {
                if (condition is not null)
                    return context.Bookings.Include(b => b.Member).Include(b => b.Session).ThenInclude(s => s.Category).Include(b => b.Session).ThenInclude(s => s.Trainer).Where(condition).ToList();
                else
                    return context.Bookings.Include(b => b.Member).Include(b => b.Session).ThenInclude(s => s.Category).Include(b => b.Session).ThenInclude(s => s.Trainer).ToList();
            }
            else 
                return null;
        }
    }
}
