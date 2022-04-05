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
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/study";
        options.LogoutPath = "/main";
    });
builder.Services.AddAuthorization();



var app = builder.Build();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
