using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MainApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Connecting a Data Context for an Application Release
#if RELEASE
    string connection = builder.Configuration.GetConnectionString("DefaultConnection");
#endif
// Connecting a Data Context for Application Testing
#if DEBUG
    string connection = builder.Configuration.GetConnectionString("TestConnection");
#endif


// Adding Data Contexts as a Service to an Application
builder.Services.AddDbContext<TopicsContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8,0,28))));
builder.Services.AddDbContext<UserContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8,0,28))));
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Auth";
        options.Cookie.HttpOnly = true;
        options.LoginPath = "/entry/userlogin";
        options.AccessDeniedPath = "/page/study";
        options.LogoutPath = "/page/welcome";
    });
builder.Services.AddAuthorization();
// Ñonverting all queries to lowercase for ease of use, for example: ~/Main/View changes to ~/main/view
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});



var app = builder.Build();
app.UseStaticFiles();

// Authentication and Authorization connection for entry
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Page}/{action=Welcome}/{id?}");

app.Run();
