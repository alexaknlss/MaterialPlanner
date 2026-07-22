using MaterialPlanner.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MaterialPlanner.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }

            return RedirectToAction("Index", "MaterialDetails");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
