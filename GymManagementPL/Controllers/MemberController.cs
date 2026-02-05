using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;
        public MemberController(IMemberService _memberService) {
            memberService = _memberService;
        }
        public IActionResult Index()
        {
            return View(memberService.GetAllMembers());
        }
        public IActionResult MemberDetails(int id)
        {
            var member = memberService.GetMemberDetails(id);
            
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member is not found!";
                return RedirectToAction(nameof(Index));
            }
            
            return View(member);
        }
        public IActionResult HealthRecordDetails(int id)
        {
            var healthRecord = memberService.GetHealthRecord(id);

            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record of that Member is not found!";
                return RedirectToAction(nameof(Index));
            }

            return View(healthRecord);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateMember(MemberCreateViewModel memberCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check missing data");
                return View(nameof(Create), memberCreateViewModel);
            }
            
            if (memberService.CreateMember(memberCreateViewModel))
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed to Create the Member, Phone Number or Email Already Exist";
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult EditMember(int id)
        {
            var memberToUpdate = memberService.GetMemberToUpdate(id);
            
            if(memberToUpdate is null)
            {
                TempData["ErrorMessage"] = "Member Not Found!";
                return RedirectToAction(nameof(Index));
            }

            return View(memberToUpdate);
        }
        [HttpPost]
        public IActionResult EditMember([FromRoute] int id, MemberUpdateViewModel memberUpdateViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("MissedData", "Check Input Data!");
                return View(memberUpdateViewModel);
            }
            if(memberService.UpdateMemberDetails(id, memberUpdateViewModel))
                TempData["SuccessMessage"] = "Member is Updated Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Update This Member!";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            if(id <= 0)
                TempData["ErrorMessage"] = "Member Id can't be zero or negative!!";
            
            var member = memberService.GetMemberDetails(id);
            if(member is null){
                TempData["ErrorMessage"] = "Member is not found!!";
                return RedirectToAction(nameof(Index));
            }

            TempData["DeleteMemberId"] = id;

            return RedirectToAction(nameof(Index));
        }
        public IActionResult DeleteMember([FromForm] int id)
        {
            if (memberService.RemoveMember(id))
                TempData["SuccessMessage"] = "Member is deleted Successfully";
            else
                TempData["ErrorMessage"] = "Failed to delete this Member!!";
            return RedirectToAction(nameof(Index));
        }
    }
}
