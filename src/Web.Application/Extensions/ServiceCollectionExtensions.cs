using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Web.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; // Optional override
                    options.AccessDeniedPath = "/Account/AccessDenied"; // Optional
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
               {
                   config.Bind("AzureEntra", options);
                   options.ResponseType = OpenIdConnectResponseType.Code;
                   options.Events ??= new OpenIdConnectEvents();
                   options.Events.OnTokenValidated += OnTokenValidatedFunc;
                   options.Events.OnAuthorizationCodeReceived += OnAuthorizationCodeReceivedFunc;
               });

            return services;
        }

        private static async Task OnAuthorizationCodeReceivedFunc(AuthorizationCodeReceivedContext context)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }

        private static async Task OnTokenValidatedFunc(TokenValidatedContext context)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
    