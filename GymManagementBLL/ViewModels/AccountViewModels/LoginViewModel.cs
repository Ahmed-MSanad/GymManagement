using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is Required!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is Required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
