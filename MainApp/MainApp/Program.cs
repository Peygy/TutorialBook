using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MainApp.Models;


var builder = WebApplication.CreateBuilder(args);

// Connecting a Data Context for an Application Release
#if RELEASE
    string connectionUser = builder.Configuration.GetConnectionString("DefaultUserConnection");
    string connectionTopics = builder.Configuration.GetConnectionString("DefaultTopicConnection");
#endif
// Connecting a Data Context for Application Testing
#if DEBUG
    string connectionUser = builder.Configuration.GetConnectionString("TestUserConnection");
    string connectionTopics = builder.Configuration.GetConnectionString("TestTopicConnection");
#endif



// Adding Data Contexts as a Service to an Application
builder.Services.AddDbContext<UserContext>(options => options.UseNpgsql(connectionUser));
builder.Services.AddDbContext<TopicsContext>(options => options.UseNpgsql(connectionTopics));
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
IHostEnvironment? env = app.Services.GetService<IHostEnvironment>();

app.UseStaticFiles();
app.UseDefaultFiles();
app.UseStatusCodePagesWithReExecute("/error/{0}");

// Node_modules folder support
if (env != null)
{
    app.UseFileServer(new FileServerOptions()
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(env.ContentRootPath, "node_modules")),
        RequestPath = "/node_modules",
        EnableDirectoryBrowsing = false
    });
}


// Authentication and Authorization connection for entry
app.UseAuthentication();
app.UseAuthorization();
// For using Sessions
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Page}/{action=Welcome}/{id?}"); 

app.Run();
