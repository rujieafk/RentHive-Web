using Microsoft.AspNetCore.Mvc;
using RentHive.Models;

namespace RentHive.Controllers
{
    public class ErrorView : Controller
    {
        public IActionResult ErrorMessage(string ErrorMessage)
        {
            ViewBag.ErrorMessage = ErrorMessage;
            // Remove the "UserData" from the session
            HttpContext.Session.Remove("UserData");
            return View();
        }
    }
}
