using GymManagementBLL.ViewModels.MembershipViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<MembershipViewModel> GetMemberShips();
    }
}
