using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController(ITrainerService trainerService) : Controller
    {
        public IActionResult Index()
        {
            return View(trainerService.GetAllTrainers());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateTrainer(TrainerCreateViewModel trainerCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check missing data");
                return View(nameof(Create), trainerCreateViewModel);
            }

            if (trainerService.CreateTrainer(trainerCreateViewModel))
                TempData["SuccessMessage"] = "Trainer Created Successfully!";
            else
                TempData["ErrorMessage"] = "Failed To Create Trainer!!";

            return RedirectToAction(nameof(Index));
        }
        public IActionResult TrainerDetails(int id)
        {
            Console.WriteLine($"Trainer Id is {id}");
            TrainerCreateViewModel trainer = trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer is not found!";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }
        public IActionResult EditTrainer(int id)
        {            
            var trainer = trainerService.GetTrainerDetails(id);

            if(trainer is null){
                TempData["ErrorMessage"] = "Faild to find the Trainer!";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }
        [HttpPost]
        public IActionResult EditTrainer([FromRoute] int id, TrainerCreateViewModel trainerCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("MissedData", "Check Missing Data!");
                return View(trainerCreateViewModel);
            }

            if(trainerService.UpdateTrainerData(id, trainerCreateViewModel))
                TempData["SuccessMessage"] = "Trainer Created Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Create a Trainer!";
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var trainer = trainerService.GetTrainerDetails(id);
            if(trainer is null)
                TempData["ErrorMessage"] = "Trainer not exist!";
            else
                TempData["TrainerId"] = id;
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteTrainer([FromForm]int id)
        {
            Console.WriteLine($"trainer id: {id}");
            if (trainerService.RemoveTrainer(id))
                TempData["SuccessMessage"] = "Trainer Deleted Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to delete Trainer!";
            return RedirectToAction(nameof(Index));
        }
    }
}
