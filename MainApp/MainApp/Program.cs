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
builder.Services.AddDbContext<TopicsContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8,0,29))));
builder.Services.AddDbContext<UserContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8,0,29))));
builder.Services.AddControllersWithViews();

// Adding Sessions to services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "AuthSession";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
});

// Adding Cookies to services
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthCookie";
        options.Cookie.HttpOnly = true;
        options.LoginPath = "/entry/userlogin";
        options.AccessDeniedPath = "/page/study";
        options.LogoutPath = "/page/welcome";
    });
builder.Services.AddAuthorization();

// Converting all queries to lowercase for easy of use, for example: ~/Main/View changes to ~/main/view
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
#if DEBUG
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
#endif



var app = builder.Build();
app.UseStaticFiles();

// Authentication and Authorization connection for entry
app.UseAuthentication();
app.UseAuthorization();
// For using Sessions
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Page}/{action=Welcome}/{id?}");

app.Run();
