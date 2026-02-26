using GymManagementBLL.ViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<BookingViewModel>? GetBookings();
        bool AddBooking(BookingCreateViewModel bookingCreateViewModel);
        bool removeBooking(int memberId, int sessionId);
    }
}
