using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeoTuna.Admin.Web.Controllers
{
    [Route("/dashboard")]
    [Authorize]
    public class DashboardController : Controller
    {
        public DashboardController()
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
