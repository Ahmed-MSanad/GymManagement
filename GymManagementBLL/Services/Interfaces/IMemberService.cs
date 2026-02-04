using GymManagementBLL.ViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {
        bool CreateMember(MemberCreateViewModel memberCreateViewModel);
        IEnumerable<MemberViewModel> GetAllMembers();
        MemberViewModel? GetMemberDetails(int id);
        HealthRecordViewModel? GetHealthRecord(int memberId);
        bool UpdateMemberDetails(int memberId, MemberUpdateViewModel memberUpdateViewModel);
        MemberUpdateViewModel? GetMemberToUpdate(int memberId);
        bool RemoveMember(int memberId);
    }
}
