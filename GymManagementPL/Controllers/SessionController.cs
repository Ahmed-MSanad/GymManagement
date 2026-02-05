using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController(ISessionService sessionService) : Controller
    {
        public IActionResult Index()
        {
            var sessions = sessionService.GetAllSessions();
            return View(sessions);
        }
        public IActionResult Create()
        {
            LoadCategories();
            LoadTrainers();
            return View();
        }
        [HttpPost]
        public IActionResult Create(SessionCreateViewModel sessionCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("MissedData", "Check Missing Data!!");
                LoadCategories();
                LoadTrainers();
                return View(sessionCreateViewModel);
            }
            if (sessionService.CreateSession(sessionCreateViewModel))
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("MissedData", "Failed to Create Session");
                LoadCategories();
                LoadTrainers();
                return View(sessionCreateViewModel);
            }
        }
        public IActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Session Id Can't be zero or negative!";
                return RedirectToAction(nameof(Index));
            }
            var session = sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not Found!";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }
        public IActionResult EditSession(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id must be positive!";
                return RedirectToAction(nameof(Index));
            }
            var session = sessionService.GetSessionToUpdate(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session can't be updated!";
                return RedirectToAction(nameof(Index));
            }
            LoadTrainers();
            return View(session);
        }
        [HttpPost]
        public IActionResult EditSession([FromRoute]int id, SessionUpdateViewModel sessionUpdateViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("MissedData", "Missing Session Data!");
                LoadTrainers();
                return View(sessionUpdateViewModel);
            }
            if(sessionService.UpdateSession(id, sessionUpdateViewModel))
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("MissedData", "Fail to Update Session!");
                LoadTrainers();
                return View(sessionUpdateViewModel);
            }
        }
        public IActionResult Delete(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "session id can't be negative or zero!";
                return RedirectToAction(nameof(Index));
            }
            
            var session = sessionService.GetSessionById(id);
            if(session is null)
                TempData["ErrorMessage"] = "session is not found!";
            else
                TempData["SessionId"] = id;
            
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult DeleteSession([FromForm]int id)
        {
            if (sessionService.RemoveSession(id))
                TempData["SuccessMessage"] = "Session is deleted successfully";
            else
                TempData["ErrorMessage"] = "Failed to delete the Session!";
            return RedirectToAction(nameof(Index));
        }
        #region Helper Methods
        private void LoadCategories()
        {
            ViewBag.Categories = new SelectList(sessionService.GetCategoriesDropdown(), "Id", "CategoryName");
        }
        private void LoadTrainers()
        {
            ViewBag.Trainers = new SelectList(sessionService.GetTrainersDropdown(), "Id", "Name");
        }
        #endregion
    }
}
