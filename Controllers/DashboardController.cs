using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TaskManagementPortal.Controllers
{
    public class DashboardController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.LoggedUserName = HttpContext.Session.GetString("LoggedUserName") ?? "Guest";
            ViewBag.LoggedUser = HttpContext.Session.GetString("LoggedUser") ?? "Guest";
            return View();
        }
    }
}
