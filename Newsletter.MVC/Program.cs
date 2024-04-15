using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Newsletter.Application;
using Newsletter.Domain.Entities;
using Newsletter.Infrastructure;
using Newsletter.Domain.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.CreateServiceTool();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(configure =>
{
    configure.Cookie.Name = "Newsletter.Auth";
    configure.LoginPath = "/Auth/Login";
    configure.LogoutPath = "/Auth/Login";
    configure.AccessDeniedPath = "/Auth/Login";
    configure.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using (var scoped = app.Services.CreateScope())
{
    var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    if (!userManager.Users.Any())
    {
        AppUser appUser = new()
        {
            Email = "cagla@gmail.com",
            UserName = "ctuncsavas"
        };

        userManager.CreateAsync(appUser, "Password12*").Wait();
    }
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
