using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork unitOfWork;
        public AnalyticsService(IUnitOfWork _unitOfWork) {
            unitOfWork = _unitOfWork;
        }
        public AnalyticsViewModel GetAnalyticsData()
        {
            var sessionRepo = unitOfWork.GetRepository<Session>();
            return new AnalyticsViewModel
            {
                TotalMembers = unitOfWork.GetRepository<Member>().GetAll().Count(),
                ActiveMembers = unitOfWork.GetRepository<MemberShip>().GetAll(m => m.Status == "Active").Count(),
                TotalTrainers = unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                CompletedSessions = sessionRepo.GetAll(s => s.EndDate < DateTime.UtcNow).Count(),
                OngoingSessions = sessionRepo.GetAll(s => (s.StartDate <= DateTime.UtcNow && s.EndDate >= DateTime.UtcNow)).Count(),
                UpcomingSessions = sessionRepo.GetAll(s => (s.StartDate > DateTime.UtcNow)).Count(),
            };
        }
    }
}
