using Microsoft.EntityFrameworkCore;
using MainApp.Models;
using MainApp.Controllers;


var builder = WebApplication.CreateBuilder(args);

// ѕодключение контекста данных дл€ релиза приложени€
#if RELEASE 
    string connection = builder.Configuration.GetConnectionString("DefaultConnection");
#endif
// ѕодключение контекста данных дл€ тестировани€ приложени€
#if DEBUG
    string connection = builder.Configuration.GetConnectionString("TestConnection");
#endif
// ƒобавление контекстов данных в качестве сервиса в приложение
builder.Services.AddDbContext<TopicsContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(connection));
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();


app.Run(async context =>
{
    app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Entry}/{action=CookieCheck}");
});


app.Run();
