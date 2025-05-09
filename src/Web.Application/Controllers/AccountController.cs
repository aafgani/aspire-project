using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.Infrastructure.Cache;
using Web.Application.Extensions;

namespace Web.Application.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutAsync()
        {
            var userId = User.GetIdentifier(); 
            var sessionId = User.GetSessionId(); 
            var sessionKey = $"session-{userId}";
            var cache = HttpContext.RequestServices.GetRequiredService<ICacheService>();

            await cache.RemoveAsync(sessionKey);

            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Login", "Account") // after logout
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
