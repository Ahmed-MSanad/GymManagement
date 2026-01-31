using GymManagementDAL.Entities.Enum;

namespace GymManagementDAL.Entities
{
    public class Trainer : GymUser
    {
        public Specialities Specialities { get; set; }
        public ICollection<Session> Sessions { get; set; } = null!;
    }
}
