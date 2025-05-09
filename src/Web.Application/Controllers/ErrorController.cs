using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace Web.Application.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult Index(string message, int statusCode)
        {
            ViewBag.StatusCode = statusCode;
            ViewBag.Message = HtmlEncoder.Default.Encode(message ?? "Something went wrong.");
            return View();
        }
    }
}
