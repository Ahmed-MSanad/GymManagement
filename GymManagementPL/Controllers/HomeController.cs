using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsService analyticsService;
        public HomeController(IAnalyticsService _analyticsService)
        {
            analyticsService = _analyticsService;
        }

        public IActionResult Index()
        {
            return View(analyticsService.GetAnalyticsData());
        }
    }
}
