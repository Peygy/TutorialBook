using Microsoft.EntityFrameworkCore;
using MainApp.Models;
using MainApp.Controllers;


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


app.Run(async context =>
{
    app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Entry}/{action=CookieCheck}");
});


app.Run();
