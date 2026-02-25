namespace GymManagementBLL.ViewModels
{
    public class MembershipViewModel
    {
        public int PlanId { get; set; }
        public int MemberId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string MemberName { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public int RemainingDays { get; set; }
    }
}
