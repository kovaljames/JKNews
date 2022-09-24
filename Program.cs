using JKNews;
using JKNews.Data;
using JKNews.Extensions;
using JKNews.Models;
using JKNews.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add EF to app
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<User, Role>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequiredLength = 8;
    // opt.Lockout.MaxFailedAccessAttempts = 5;
    // opt.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.Cookie.Name = "AppJKNewsUsers";
    opt.ExpireTimeSpan = TimeSpan.FromDays(7);
    opt.LoginPath = "/Account/Login";
    opt.LogoutPath = "/Home/Index";
    opt.AccessDeniedPath = "/Account/RestrictAccess";
    opt.ReturnUrlParameter = "returnUrl";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Seed Users and Roles
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<Role>>();
    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<User>>();
    AppDbInitializer.SeedRolesAndUsers(userManager, roleManager);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
