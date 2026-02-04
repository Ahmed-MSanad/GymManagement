using GymManagementBLL.ViewModels;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IPlanService
    {
        bool UpdatePlan(int planId, PlanUpdateViewModel planUpdateViewModel);
        PlanUpdateViewModel? GetPlanToUpdate(int planId);
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanById(int planId);
        bool ToggleActivation(int planId);
    }
}
