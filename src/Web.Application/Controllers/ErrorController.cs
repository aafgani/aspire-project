using Microsoft.AspNetCore.Mvc;

namespace Web.Application.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult Index(string message, int statusCode)
        {
            ViewBag.StatusCode = statusCode;
            ViewBag.Message = message ?? "An unknown error occurred.";
            return View(); // return View("Index") if using Views/Error/Index.cshtml
        }
    }
}
