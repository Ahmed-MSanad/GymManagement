using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.HealthRecordViewModels;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        /*
         * Email and phone must be unique and Valid.
         * Cannot delete members with active bookings.
         * Egyptian phone validation: (010|011 |012|015)XXXXXXXX.
         * Health records are required during registration.
         * Join Date Will Be Calculated Automatically.
         */
        private readonly IUnitOfWork unitOfWork;
        public MemberService(IUnitOfWork _unitOfWork) {
            unitOfWork = _unitOfWork;
        }

        public bool CreateMember(MemberCreateViewModel memberCreateViewModel)
        {
            try
            {
                if (IsEmailExist(memberCreateViewModel.Email) || IsPhoneExist(memberCreateViewModel.Phone)) return false;

                var member = new Member()
                {
                    Address = new Address
                    {
                        BuildingNumber = memberCreateViewModel.BuildingNumber,
                        City = memberCreateViewModel.City,
                        Street = memberCreateViewModel.Street,
                    },
                    HealthRecord = new HealthRecord
                    {
                        BloodType = memberCreateViewModel.HealthRecord.BloodType,
                        Note = memberCreateViewModel.HealthRecord.Note,
                        Weight = memberCreateViewModel.HealthRecord.Weight,
                        Height = memberCreateViewModel.HealthRecord.Height,
                    },
                    Email = memberCreateViewModel.Email,
                    Gender = memberCreateViewModel.Gender,
                    Phone = memberCreateViewModel.Phone,
                    DateOfBirth = memberCreateViewModel.DateOfBirth,
                    Name = memberCreateViewModel.Name,
                };
                unitOfWork.GetRepository<Member>().Add(member);

                unitOfWork.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = unitOfWork.GetRepository<Member>().GetAll() ?? [];

            if (members is null || !members.Any())
            {
                return [];
            }

            var memberViewModel = members.Select(m => new MemberViewModel
            {
                Name = m.Name,
                Id = m.Id,
                Phone = m.Phone,
                Email = m.Email,
                Photo = m.Photo,
                DateOfBirth = m.DateOfBirth.ToShortDateString(),
                Gender = m.Gender.ToString(),
            });

            return memberViewModel;
        }

        public MemberViewModel? GetMemberDetails(int id)
        {
            var member = unitOfWork.GetRepository<Member>().GetById(id);
            if (member is null) return null;

            var memberViewModel = new MemberViewModel
            {
                Name = member.Name,
                Id = member.Id,
                Phone = member.Phone,
                Email = member.Email,
                Photo = member.Photo,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = member.Address.ToString(),
            };
            // Only 1 plan is active per member plans and is ddetermined through the status membership property
            var activeMembership = unitOfWork.GetRepository<MemberShip>().GetAll(ms => ms.MemberId == id && ms.Status == "Active").FirstOrDefault(); 
            if (activeMembership is not null)
            {
                memberViewModel.MembershipStartDate = activeMembership.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = activeMembership.EndDate.ToShortDateString();
                
                var activePlan = unitOfWork.GetRepository<Plan>().GetById(activeMembership.PlanId);
                memberViewModel.PlanName = activePlan?.Name;
            }

            return memberViewModel;
        }

        public HealthRecordViewModel? GetHealthRecord(int memberId)
        {
            var memberHealthRecord = unitOfWork.GetRepository<HealthRecord>().GetById(memberId);
            if (memberHealthRecord is null) return null;

            return new HealthRecordViewModel
            {
                BloodType = memberHealthRecord.BloodType,
                Height = memberHealthRecord.Height,
                Note = memberHealthRecord.Note,
                Weight = memberHealthRecord.Weight,
            };
        }

        public bool UpdateMemberDetails(int memberId, MemberUpdateViewModel memberUpdateViewModel)
        {
            try
            {
                if (IsEmailExist(memberUpdateViewModel.Email) || IsPhoneExist(memberUpdateViewModel.Phone)) return false;
                
                var member = unitOfWork.GetRepository<Member>().GetById(memberId);
                if (member is null) return false;

                member.Address = new Address
                {
                    BuildingNumber = memberUpdateViewModel.BuildingNumber,
                    City = memberUpdateViewModel.City,
                    Street = memberUpdateViewModel.Street,
                };
                member.Name = memberUpdateViewModel.Name;
                member.Email = memberUpdateViewModel.Email;
                member.Photo = memberUpdateViewModel.Photo;
                member.Phone = memberUpdateViewModel.Phone;

                unitOfWork.GetRepository<Member>().Update(member);

                unitOfWork.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public MemberUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var memeber = unitOfWork.GetRepository<Member>().GetById(memberId);
            if (memeber is null) return null;

            return new MemberUpdateViewModel
            {
                Name = memeber.Name,
                Email = memeber.Email,
                Phone = memeber.Phone,
                Photo = memeber.Photo,
                City = memeber.Address.City,
                BuildingNumber = memeber.Address.BuildingNumber,
                Street = memeber.Address.Street,
            };
        }

        public bool RemoveMember(int memberId)
        {
            var member = unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return false;

            if (unitOfWork.GetRepository<Booking>().GetAll(b => b.MemberId == member.Id && DateTime.UtcNow < b.Session.StartDate).Any()) return false;

            var memberShips = unitOfWork.GetRepository<MemberShip>().GetAll(ms => ms.MemberId == member.Id).ToList();
            try
            {
                if (memberShips.Any())
                    memberShips.ForEach(ms => unitOfWork.GetRepository<MemberShip>().Delete(ms));

                unitOfWork.GetRepository<Member>().Delete(member);

                unitOfWork.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods
        private bool IsEmailExist(string email)
        {
            var member = unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email);
            return member is not null && member.Any();
        }
        private bool IsPhoneExist(string phone)
        {
            var member = unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone);
            return member is not null && member.Any();
        }
        #endregion
    }
}
