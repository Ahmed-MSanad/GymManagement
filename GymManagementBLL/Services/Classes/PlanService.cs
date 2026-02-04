using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService(IUnitOfWork unitOfWork) : IPlanService
    {
        /* Can't update/deactivate plans with active memberships */
        /* Plans can be activated/deactivated */
        /* Duration: 1-365 days */
        public bool UpdatePlan(int planId, PlanUpdateViewModel planUpdateViewModel)
        {
            try
            {
                var plan = unitOfWork.GetRepository<Plan>().GetById(planId);
                if (plan is null || DoesPlanHasActiveMembership(planId)) return false;

                plan.Description = planUpdateViewModel.Description;
                plan.Price = planUpdateViewModel.Price;
                plan.DurationDays = planUpdateViewModel.DurationDays;
                plan.Name = planUpdateViewModel.PlanName;

                //(plan.Description, plan.Price, plan.DurationDays, plan.Name) = 
                //    (planUpdateViewModel.Description, planUpdateViewModel.Price, planUpdateViewModel.DurationDays, planUpdateViewModel.PlanName);

                unitOfWork.GetRepository<Plan>().Update(plan);

                return unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public PlanUpdateViewModel? GetPlanToUpdate(int planId)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || plan.IsActive == false) return null; // able to update only active plan

            return new PlanUpdateViewModel
            {
                Description = plan.Description,
                Price = plan.Price,
                DurationDays = plan.DurationDays,
                PlanName = plan.Name
            };
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any()) return [];

            return plans.Select(p => new PlanViewModel
            {
                Description = p.Description,
                DurationDays = p.DurationDays,
                Id = p.Id,
                IsActive = p.IsActive,
                Name = p.Name,
                Price = p.Price,
            });
        }

        public PlanViewModel? GetPlanById(int planId)
        {
            var plan = unitOfWork.GetRepository<Plan>().GetById(planId);
            if(plan is null) return null;

            return new PlanViewModel
            {
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Id = plan.Id,
                IsActive = plan.IsActive,
                Name = plan.Name,
                Price = plan.Price,
            };
        }

        public bool ToggleActivation(int planId)
        {
            var planRepo = unitOfWork.GetRepository<Plan>();
            
            var plan = planRepo.GetById(planId);
            if(plan is null || DoesPlanHasActiveMembership(planId)) return false;

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.UtcNow;

            planRepo.Update(plan);

            return unitOfWork.SaveChanges() > 0;
        }

        #region Helper Methods

        private bool DoesPlanHasActiveMembership(int planId) 
            => unitOfWork.GetRepository<MemberShip>().GetAll(ms => ms.PlanId == planId && ms.Status == "Active").Any();

        #endregion
    }
}
