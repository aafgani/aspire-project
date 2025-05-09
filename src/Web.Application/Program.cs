using App.Infrastructure.Extensions;
using Microsoft.Identity.Web.UI;
using Web.Application.Extensions;
using Web.Application.Utility.Middleware;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services
    .AddAuthentication(config)
    .AddInfrastructureServicesForWebApp();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}    

//To ensure that any incoming HTTP request will automatically be redirected to HTTPS.
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<SessionValidationMiddleware>();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
