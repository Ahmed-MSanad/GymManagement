using System.ComponentModel.DataAnnotations;
using GymManagementDAL.Entities.Enum;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
    public class TrainerCreateViewModel
    {
        public string Name { get; set; } = null!;
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage ="Email is not valid")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^01(0|1|2|5)\d{8}$", ErrorMessage ="Invalid Egyption Phone number, please try again!")]
        public string Phone { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public int BuildingNumber { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = "Speciality is required")]
        public Specialities Specialities { get; set; }
    }
}
