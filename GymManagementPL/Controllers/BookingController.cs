using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class BookingController(IBookingService bookingService, IMemberService memberService, ISessionService sessionService) : Controller
    {
        public IActionResult Index()
        {
            return View(bookingService.GetBookings());
        }
        public IActionResult Create()
        {
            ViewBag.MembersList = new SelectList(memberService.GetAllMembers(), "Id", "Name");
            ViewBag.SessionsList = new SelectList(sessionService.GetAllSessions(), "Id", "DisplayName");
            return View();
        }
        [HttpPost]
        public IActionResult Create(BookingCreateViewModel bookingCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["InValidInput"] = "Please correct the errors in the form.";
                return RedirectToAction("Create");
            }
            if (bookingService.AddBooking(bookingCreateViewModel))
            {
                return RedirectToAction("Index");
            }
            TempData["CreationFailed"] = "Failed to create booking.";
            return RedirectToAction("Create");
        }
        public IActionResult Delete(int memberId, int sessionId)
        {
            TempData["memberId"] = memberId;
            TempData["sessionId"] = sessionId;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteBooking([FromForm] int memberId, [FromForm] int sessionId)
        {
            if(!bookingService.removeBooking(memberId, sessionId))
                TempData["DeletionFailed"] = "Failed to delete booking. Only future bookings can be cancelled.";
            return RedirectToAction("Index");
        }
    }
}
