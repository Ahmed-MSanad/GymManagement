using GymManagementBLL.ViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetMemberShips();
        bool AddMembership(MembershipCreateViewModel membershipCreateViewModel);
        bool RemoveMembership(int memberId, int planId);
    }
}
