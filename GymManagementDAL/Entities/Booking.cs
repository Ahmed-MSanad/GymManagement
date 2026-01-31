namespace GymManagementDAL.Entities
{
    public class Booking : BaseEntity
    {
        public bool IsAttended { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}
