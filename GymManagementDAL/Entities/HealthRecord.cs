using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementDAL.Entities
{
    [Table("Members")] // This is the table name that the class is mapped to
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }
    }
}
