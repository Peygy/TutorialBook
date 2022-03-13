using Microsoft.EntityFrameworkCore;
using MainApp.Models;


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

app.Run(async (context) =>
{
    if (context.Request.Cookies.ContainsKey("login"))
    {
        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Welcome}/{id?}");
        });
    }
    else
    {
        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Entry}/{action=SingIn}/{id?}");
        });
    }
});


app.Run();
