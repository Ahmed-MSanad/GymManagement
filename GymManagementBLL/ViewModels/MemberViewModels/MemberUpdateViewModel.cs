using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels
{
    public class MemberUpdateViewModel
    {
        public string? Photo { get; set; }

        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email must be a valid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone is required.")]
        [Phone(ErrorMessage = "Phone must be a valid phone number.")]
        [RegularExpression(@"^01(0|1|2|5)\d{8}$", ErrorMessage = "Phone number must be valid egyption phone number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Building Number must be greater than 0")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "City Is Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 150 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Street can only contain letters, numbers, and spaces")]
        public string Street { get; set; } = null!;
    }
}
