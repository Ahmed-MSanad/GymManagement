using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class BookingService(IUnitOfWork unitOfWork, IMapper mapper) : IBookingService
    {
        public IEnumerable<BookingViewModel>? GetBookings()
        {
            var bookings = unitOfWork.BookingRepository.GetBookingsWithDetails();
            if(bookings is not null && bookings.Any())
            {
                return mapper.Map<IEnumerable<BookingViewModel>>(bookings);
            }
            return null;
        }
        public bool AddBooking(BookingCreateViewModel bookingCreateViewModel)
        {
            /* Member must have active membership to book a session */
            var activeMembership = unitOfWork.MemberShipRepository.GetAll(ms => ms.MemberId == bookingCreateViewModel.MemberId && ms.Status == "Active").FirstOrDefault();
            if (activeMembership is null)
                return false;
            /* Session must have active capacity */
            var session = unitOfWork.SessionRepository.GetById(bookingCreateViewModel.SessionId);
            if (session is null || session.Capacity <= 0)
                return false;
            /* Member can't book the same session twice */
            var existingBooking = unitOfWork.BookingRepository.GetAll(b => b.MemberId == bookingCreateViewModel.MemberId && b.SessionId == bookingCreateViewModel.SessionId).FirstOrDefault();
            if(existingBooking is not null)
                return false;
            /* Only future sessions can be booked */
            if (session.StartDate <= DateTime.Now)
                return false;

            Booking booking = new Booking
            {
                MemberId = bookingCreateViewModel.MemberId,
                SessionId = bookingCreateViewModel.SessionId,
                IsAttended = false
            };

            unitOfWork.BookingRepository.Add(booking);

            return unitOfWork.SaveChanges() > 0;
        }
        public bool removeBooking(int memberId, int sessionId)
        {
            /* Only future bookings can be cancelled */
            var booking = unitOfWork.BookingRepository.GetAll(b => b.MemberId == memberId && b.SessionId == sessionId).FirstOrDefault();
            if(booking is null) 
                return false;
            if(booking.Session.StartDate <= DateTime.Now)
                return false;

            unitOfWork.BookingRepository.Delete(booking);

            return unitOfWork.SaveChanges() > 0;
        }
    }
}
