using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager) : Controller
    {
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid User Login!");
                return View(loginViewModel);
            }

            var user = accountService.ValidateUser(loginViewModel);

            if(user == null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid User Login!");
                return View(loginViewModel);
            }

            var result = signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false).Result;

            if(result.IsNotAllowed)
                ModelState.AddModelError("InvalidLogin", "Your Account is Not Allowed!");
            if(result.IsLockedOut)
                ModelState.AddModelError("InvalidLogin", "Your Account is Locked out!");
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return View(loginViewModel);
        }
        public IActionResult Logout()
        {
            signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
