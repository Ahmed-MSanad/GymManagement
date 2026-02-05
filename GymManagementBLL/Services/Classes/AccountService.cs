using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagementBLL.Services.Classes
{
    public class AccountService(UserManager<ApplicationUser> userManager) : IAccountService
    {
        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
            var user = userManager.FindByEmailAsync(loginViewModel.Email).Result;

            if (userManager.CheckPasswordAsync(user, loginViewModel.Password).Result)
                return user;
            else
                return null;
        }
    }
}
