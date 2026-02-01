using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels.HealthRecordViewModels
{
    public class HealthRecordViewModel
    {
        [Range(0.1, 300, ErrorMessage ="Height must be between 0.1 and 300")]
        public decimal Height { get; set; }
        [Range(0.1, 500, ErrorMessage ="Weight must be between 0.1Kg and 500Kg")]
        public decimal Weight { get; set; }
        [Required(ErrorMessage ="Blood Type is required")]
        [StringLength(3, ErrorMessage ="Blood type must be 3 characters or less")]
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }
    }
}
