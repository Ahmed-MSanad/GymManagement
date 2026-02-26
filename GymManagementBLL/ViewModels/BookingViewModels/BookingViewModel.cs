namespace GymManagementBLL.ViewModels
{
    public class BookingViewModel
    {
        public int MemberId { get; set; }
        public int SessionId { get; set; }
        public int TrainerId { get; set; }
        public bool IsAttended { get; set; }
        public string MemberName { get; set; }
        public string CategoryName { get; set; }
        public string TrainerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
