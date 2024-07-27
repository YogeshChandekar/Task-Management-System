using Microsoft.AspNetCore.Authentication.Cookies;
using FluentValidation;
using FluentValidation.AspNetCore;
using TaskManagementPortal;
using TaskManagementPortal.CommonRequestHandler;
using TaskManagementPortal.Models.LoginManagement.LoginModel;

var builder = WebApplication.CreateBuilder(args);

// Configuration and service registration
GlobalOptions.PublicKey = builder.Configuration.GetSection("MySettings").GetSection("PublicKey").Value;

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Login/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.Cookie.Name = "28f7776fa3b890a713ec04d093a2afcd6cf50276e69288056ac5f725f44bae98";
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "3cf33913ae42372f8e635e66cbb0e9987a265f52be6554be058f83cc97dbdf68";
    options.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddHttpClient("tmsService");


builder.Services.AddScoped<ICommanWebRequestHandler, CommanWebRequestHandler>();

builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

