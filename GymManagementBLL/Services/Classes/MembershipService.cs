using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class MembershipService(IUnitOfWork unitOfWork, IMapper mapper) : IMembershipService
    {
        public IEnumerable<MembershipViewModel> GetMemberShips()
        {
            var memberShips = unitOfWork.MemberShipRepository.GetAllMemberShipsIncludingPlansAndMembers();

            var result = mapper.Map<IEnumerable<MembershipViewModel>>(memberShips);

            return result;
        }

        public bool AddMembership(MembershipCreateViewModel membershipCreateViewModel)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(membershipCreateViewModel.PlanId);
            if (plan is null) return false;

            MemberShip memberShip = new MemberShip
            {
                EndDate = DateTime.Now.AddDays(plan.DurationDays),
                MemberId = membershipCreateViewModel.MemberId,
                PlanId = membershipCreateViewModel.PlanId
            };

            unitOfWork.MemberShipRepository.Add(memberShip);

            unitOfWork.SaveChanges();

            return true;
        }
    
        public bool RemoveMembership(int memberId, int planId)
        {
            var memberShip = unitOfWork.MemberShipRepository.GetAll(ms => ms.MemberId == memberId && ms.PlanId == planId).FirstOrDefault();
            
            unitOfWork.MemberShipRepository.Delete(memberShip);

            unitOfWork.SaveChanges();

            return true;
        }
    }
}
