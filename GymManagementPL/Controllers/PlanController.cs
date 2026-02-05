using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController(IPlanService planService) : Controller
    {
        public IActionResult Index()
        {
            return View(planService.GetAllPlans());
        }
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Id, Id must be positive!";
                return RedirectToAction(nameof(Index));
            }
            var plan = planService.GetPlanById(id);

            if(plan is null)
            {
                TempData["ErrorMessage"] = "Plan is not Avaliable!";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);
        }
        public IActionResult EditPlan(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Id must be positive!";
                return RedirectToAction(nameof(Index));
            }
            var plan = planService.GetPlanToUpdate(id);
            if(plan is null)
            {
                TempData["ErrorMessage"] = "Plan can't be updated!";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpPost]
        public IActionResult EditPlan([FromRoute]int id, PlanUpdateViewModel planUpdateViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check data missing!");
                return View(planUpdateViewModel);
            }

            if (!planService.UpdatePlan(id, planUpdateViewModel))
                TempData["ErrorMessage"] = "Faild to Update Plan!";
            else
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult ToggleActivation(int id)
        {
            if (planService.ToggleActivation(id))
                TempData["SuccessMessage"] = "Plan status changed!";
            else
                TempData["ErrorMessage"] = "Failed to change Plan status!";
            return RedirectToAction(nameof(Index));
        }
    }
}
