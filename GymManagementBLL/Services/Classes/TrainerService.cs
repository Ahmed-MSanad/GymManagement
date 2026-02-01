using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService(UnitOfWork unitOfWork) : ITrainerService
    {
        /*
         * Email and phone must be unique and Valid.
         * Cannot delete trainers with future sessions.
         * Must have one specialty assigned.
         * Hire Date Will Be Calculated Automatically.
         */
        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = unitOfWork.GetRepository<Trainer>().GetAll();
            if (trainers is null || !trainers.Any()) return [];

            return trainers.Select(t => new TrainerViewModel
            {
                Email = t.Email,
                Name = t.Name,
                Phone = t.Phone,
                Specialities = t.Specialities.ToString(),
            });
        }

        public bool CreateTrainer(TrainerCreateViewModel trainerCreateViewModel)
        {
            try
            {
                if (isEmailExist(trainerCreateViewModel.Email) || isPhoneExist(trainerCreateViewModel.Phone))
                    return false;

                var trainer = new Trainer
                {
                    Email = trainerCreateViewModel.Email,
                    Name = trainerCreateViewModel.Name,
                    Phone = trainerCreateViewModel.Phone,
                    DateOfBirth = trainerCreateViewModel.DateOfBirth,
                    Gender = trainerCreateViewModel.Gender,
                    Address = new Address
                    {
                        BuildingNumber = trainerCreateViewModel.BuildingNumber,
                        City = trainerCreateViewModel.City,
                        Street = trainerCreateViewModel.Street
                    },
                    Specialities = trainerCreateViewModel.Specialities
                };
                unitOfWork.GetRepository<Trainer>().Add(trainer);

                unitOfWork.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public TrainerCreateViewModel? GetTrainerDetails(int trainerId)
        {
            var trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (trainer is null) return null;

            return new TrainerCreateViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Gender = trainer.Gender,
                DateOfBirth = trainer.DateOfBirth,
                Specialities = trainer.Specialities,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street,
            };
        }

        public bool UpdateTrainerData(int trainerId, TrainerCreateViewModel trainerCreateViewModel)
        {
            try
            {
                if (isEmailExist(trainerCreateViewModel.Email) || isPhoneExist(trainerCreateViewModel.Phone)) return false;

                var trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
                if(trainer is null) return false;

                trainer.Email = trainerCreateViewModel.Email;
                trainer.Specialities = trainerCreateViewModel.Specialities;
                trainer.Address = new Address
                {
                    BuildingNumber = trainerCreateViewModel.BuildingNumber,
                    City = trainerCreateViewModel.City,
                    Street = trainerCreateViewModel.Street,
                };
                trainer.Phone = trainerCreateViewModel.Phone;

                unitOfWork.GetRepository<Trainer>().Update(trainer);

                unitOfWork.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveTrainer(int trainerId)
        {
            try
            {
                var trainer = unitOfWork.GetRepository<Trainer>().GetById(trainerId);
                if (trainer is null) return false;

                if (unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == trainerId && s.StartDate > DateTime.UtcNow).Any()) return false;

                unitOfWork.GetRepository<Trainer>().Delete(trainer);

                unitOfWork.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods
        private bool isEmailExist(string email)
        {
            var trainerEmail = unitOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email);
            return trainerEmail is not null && trainerEmail.Any();
        }
        private bool isPhoneExist(string phone)
        {
            var trainerPhone = unitOfWork.GetRepository<Trainer>().GetAll(t => t.Phone == phone);
            return trainerPhone is not null && trainerPhone.Any();
        }
        #endregion
    }
}
