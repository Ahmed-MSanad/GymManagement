using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService(IUnitOfWork unitOfWork, IMapper mapper) : ISessionService
    {
        /*
         * Capacity limited to 1-25 participants (enforced by Db constraint).
         * End date must be after start date.
         * A valid Trainer and Category must be assigned to each session.
         * Cannot delete sessions with future dates.
         * Cannot Update uptodate or started sessions or sessions with active bookings.
         */
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = unitOfWork.SessionRepository
                                    .GetAllSessionsWithCategoryAndTrainer()
                                    .OrderByDescending(s => s.StartDate);
            if (sessions is null || !sessions.Any()) return [];

            var mappedSessions = mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = unitOfWork.SessionRepository.GetFreeSessionSlots(session.Id);
            }

            return mappedSessions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session = unitOfWork.SessionRepository.GetSessionWithCategoryAndTrainer(sessionId);
            if (session is null) return null;

            var mappedSession = mapper.Map<SessionViewModel>(session);

            mappedSession.AvailableSlots = unitOfWork.SessionRepository.GetFreeSessionSlots(sessionId);

            return mappedSession;
        }

        public bool CreateSession(SessionCreateViewModel sessionCreateViewModel)
        {
            try
            {
                if (!isTrainerExist(sessionCreateViewModel.TrainerId) ||
                    !isCategoryExist(sessionCreateViewModel.CategoryId) ||
                    !isValidDate(sessionCreateViewModel.StartDate, sessionCreateViewModel.EndDate)) return false;

                Session session = mapper.Map<Session>(sessionCreateViewModel);

                unitOfWork.SessionRepository.Add(session);
                
                return unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateSession(int sessionId, SessionUpdateViewModel sessionUpdateViewModel)
        {
            try
            {
                var session = unitOfWork.SessionRepository.GetById(sessionId);

                if (!isSessionAvailableForUpdate(session)) return false;

                if (!isTrainerExist(sessionUpdateViewModel.TrainerId)) return false;

                if (!isValidDate(sessionUpdateViewModel.StartDate, sessionUpdateViewModel.EndDate)) return false;

                session.StartDate = sessionUpdateViewModel.StartDate;
                session.TrainerId = sessionUpdateViewModel.TrainerId;
                session.EndDate = sessionUpdateViewModel.EndDate;
                session.Description = sessionUpdateViewModel.Description;
                session.UpdatedAt = DateTime.UtcNow;

                unitOfWork.SessionRepository.Update(session);

                return unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public SessionUpdateViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = unitOfWork.SessionRepository.GetById(sessionId);
            if(session is null) return null;

            return mapper.Map<SessionUpdateViewModel>(session);
        }

        public bool RemoveSession(int sessionId)
        {
            var session = unitOfWork.SessionRepository.GetById(sessionId);
            if (!isSessionAvailableForRemove(session)) return false;

            unitOfWork.SessionRepository.Delete(session);
            return unitOfWork.SaveChanges() > 0;
        }

        public IEnumerable<CategorySelectViewModel> GetCategoriesDropdown()
        {
            return mapper.Map<IEnumerable<CategorySelectViewModel>>(unitOfWork.GetRepository<Category>().GetAll());
        }
        public IEnumerable<TrainerSelectViewModel> GetTrainersDropdown()
        {
            return mapper.Map<IEnumerable<TrainerSelectViewModel>>(unitOfWork.GetRepository<Trainer>().GetAll());
        }

        #region Helper Methods
        private bool isTrainerExist(int trainerId)
            => unitOfWork.GetRepository<Trainer>().GetById(trainerId) is null ? false : true;
        private bool isCategoryExist(int categoryId)
            => unitOfWork.GetRepository<Category>().GetById(categoryId) is null ? false : true;
        private bool isValidDate(DateTime startDate, DateTime endDate)
            => endDate > startDate && startDate > DateTime.UtcNow;
        private bool isSessionAvailableForUpdate(Session session)
            => !(session is null)
                &&
                !(session.EndDate < DateTime.UtcNow) // not ended session
                &&
                !(session.StartDate <= DateTime.UtcNow) // not started session
                &&
                unitOfWork.SessionRepository.GetFreeSessionSlots(session.Id) == session.Capacity; // not session with active members
        private bool isSessionAvailableForRemove(Session session)
        => !(session is null)
            &&
            !(session.StartDate > DateTime.UtcNow) // not upcoming session
            &&
            !(session.StartDate <= DateTime.UtcNow && session.EndDate >= DateTime.UtcNow) // not currently working session
            &&
            unitOfWork.SessionRepository.GetFreeSessionSlots(session.Id) == session.Capacity; // not session with active members
        #endregion
    }
}
