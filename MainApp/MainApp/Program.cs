using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MainApp.Models;


var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Äîáàâëåíèå êîíòåêñòîâ äàííûõ â êà÷åñòâå ñåðâèñà â ïðèëîæåíèå
builder.Services.AddDbContext<TopicsContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8,0,28))));
builder.Services.AddDbContext<UserContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8,0,28))));
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Auth";
        options.Cookie.HttpOnly = true;
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/main";
        options.LogoutPath = "/welcome";
    });
builder.Services.AddAuthorization();



var app = builder.Build();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
