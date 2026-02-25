using GymManagementBLL.ViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetMemberShips();
        bool AddMemberShip(MembershipCreateViewModel membershipCreateViewModel);
    }
}
