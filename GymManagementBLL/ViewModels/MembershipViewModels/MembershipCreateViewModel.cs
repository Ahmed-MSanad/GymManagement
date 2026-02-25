using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels
{
    public class MembershipCreateViewModel
    {
        [Required(ErrorMessage = "Please select a member.")]
        public int MemberId { get; set; }
        [Required(ErrorMessage = "Please select a plan.")]
        public int PlanId { get; set; }
    }
}
