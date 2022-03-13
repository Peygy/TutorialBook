using Microsoft.EntityFrameworkCore;
using MainApp.Models;


var builder = WebApplication.CreateBuilder(args);

// ����������� ��������� ������ ��� ������ ����������
#if RELEASE 
    string connection = builder.Configuration.GetConnectionString("DefaultConnection");
#endif
// ����������� ��������� ������ ��� ������������ ����������
#if DEBUG
    string connection = builder.Configuration.GetConnectionString("TestConnection");
#endif
// ���������� ���������� ������ � �������� ������� � ����������
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
