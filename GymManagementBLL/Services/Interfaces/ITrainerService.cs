using GymManagementBLL.ViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainer(TrainerCreateViewModel trainerCreateViewModel);
        TrainerCreateViewModel? GetTrainerDetails(int trainerId); // can also be used for Get Trainer To Update
        bool UpdateTrainerData(int trainerId, TrainerCreateViewModel trainerCreateViewModel);
        bool RemoveTrainer(int trainerId);
    }
}
