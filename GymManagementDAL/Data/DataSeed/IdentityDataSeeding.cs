using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManagementDAL.Data.DataSeed
{
    public static class IdentityDataSeeding
    {
        public static bool SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            try
            {
                if (!roleManager.Roles.Any())
                {
                    var roles = new List<IdentityRole>()
                    {
                        new IdentityRole(){ Name = "SuperAdmin" },
                        new IdentityRole(){ Name = "Admin" }
                    };
                    foreach(var role in roles)
                    {
                        if (!roleManager.RoleExistsAsync(role.Name).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }
                    }
                }
                if (!userManager.Users.Any())
                {
                    var SuperAdmin = new ApplicationUser
                    {
                        FirstName = "Ahmed",
                        LastName = "Khaled",
                        UserName = "AhmedKhaled",
                        Email = "ahmedKhaled@gmail.com",
                        PhoneNumber = "01234567890",
                    };
                    userManager.CreateAsync(SuperAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(SuperAdmin, "SuperAdmin").Wait();
                    var Admin = new ApplicationUser
                    {
                        FirstName = "Omar",
                        LastName = "Samir",
                        UserName = "OmarSamir",
                        Email = "omarSamir@gmail.com",
                        PhoneNumber = "01098765431",
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Identity Seeding Failed!");
                return false;
            }
        }
    }
}
