using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MembershipController(IMembershipService membershipService, IPlanService planService, IMemberService memberService) : Controller
    {
        public IActionResult Index()
        {
            var result = membershipService.GetMemberShips();
            return View(result);
        }
        public IActionResult Create()
        {
            ViewBag.PlansList = new SelectList(planService.GetAllPlans(), "Id", "Name");

            ViewBag.MembersList = new SelectList(memberService.GetAllMembers(), "Id", "Name");
            
            return View();
        }
        [HttpPost]
        public IActionResult Create(MembershipCreateViewModel membershipCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("InValidInput", "Please Select Valid Values!");
                return View(membershipCreateViewModel);
            }
            bool result = membershipService.AddMembership(membershipCreateViewModel);
            if (result)
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("CreationFailed", "Failed to Create Membership. The Member already has an active plan!");
                return View();
            }
        }
        public IActionResult Delete(int memberId, int planId)
        {
            TempData["memberId"] = memberId;
            TempData["planId"] = planId;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteMembership([FromForm] int memberId, [FromForm] int planId)
        {
            membershipService.RemoveMembership(memberId, planId);
            return RedirectToAction("Index");
        }
    }
}
