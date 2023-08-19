using Best_web_application_ever.Middleware;
using Best_web_application_ever.Services;
using System.Text.RegularExpressions;


using Best_web_application_ever.Model.Data;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<ApplicationContext>();

// AddTransient � ASP .NET - ��� �����, ������� ������������ ��� ����������� ������� � ���������� ��������� ������������ � ��������� ��������� ������.
// �� �������� �� �������� ������ ���������� ������� ������ ���, ����� �� �������������!

builder.Services.AddTransient<ShortTimeService>();
builder.Services.AddTransient<ITimeService, LongTimeService>();
builder.Services.AddTransient<HashService>();

// Transient: ��� ������ ��������� � ������� ��������� ����� ������ �������.
// � ������� ������ ������� ����� ���� ��������� ��������� � �������, �������������� ��� ������ ��������� ����� ����������� ����� ������.
// �������� ������ ���������� ����� �������� �������� ��� ����������� ��������, ������� �� ������ ������ � ���������

// Scoped: ��� ������� ������� ��������� ���� ������ �������.
// �� ���� ���� � ������� ������ ������� ���� ��������� ��������� � ������ �������, �� ��� ���� ���� ���������� ����� �������������� ���� � ��� �� ������ �������.

// Singleton: ������ ������� ��������� ��� ������ ��������� � ����, ��� ����������� ������� ���������� ����
// � ��� �� ����� ��������� ������ �������

// ����� ��� ���
// builder.Services.AddTransient<ITimeService, LongTimeService>();
// � ����� ���������� ��� app.Services.GetService<ITimeService>();


// ������� ������� �������������� (������������ ������� ����������� ������������)
builder.Services.AddAuthentication("Cookies")
    .AddCookie( options => 
    { 
        options.LoginPath = "/login"; 
        options.LogoutPath = "/logout";
    } );

// ��������� ����������� (������������ ������� �����������, ����� �� ������������ ����� ������� � ���������� �������) 
builder.Services.AddAuthorization();

builder.Services.AddSingleton<ICountService, SomePlusPlusService>();

builder.Services.AddHostedService<CringeCountService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();




// ���������� middleware �������������� 
app.UseAuthentication();   

// ���������� middleware ����������� 
app.UseAuthorization();


app.UseMiddleware<LoginMidleware>();
app.UseMiddleware<RegisterMidleware>();


app.UseMiddleware<UserApiMiddleware>();

app.MapGet("/logout", async (HttpContext context) =>
{
    var Responce = context.Response;
    var cookies = Responce.Cookies;
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    cookies.Delete("user_name");
    return Results.Redirect("/login");
});

app.Map("/hello", [Authorize] () => "Hello World!");

app.Map("/Amogus", async (HttpContext context) =>
{

    var Responce = context.Response;
    await Responce.WriteAsync("hi i am amgus mice to meet you");

});


app.Map("/header", async (HttpContext context) =>
{

    var Responce = context.Response;
    Responce.ContentType = "text/html; charset=utf-8";
    await Responce.SendFileAsync("wwwroot/header.php");

});




app.Run();

// ��������� ���� �������������




// idk wtf sheet

//Run(): ��������� ����������

//RunAsync(): ���������� ��������� ����������

//Start(): ��������� ����������

//StartAsync(): ��������� ����������

//StopAsync(): ������������� ����������




// __________ also sheet __________ 

//  � ������ WebApplicationBuilder ���������� ��������� ��������:

//Configuration: ������������ ������ ConfigurationManager, ������� ����������� ��� ���������� ������������ � ����������.

//Environment: ������������� ���������� �� ���������, � ������� �������� ����������.

//Host: ������ IHostBuilder, ������� ����������� ��� ��������� �����.

//Logging: ��������� ���������� ��������� ������������ � ����������.

//Services: ������������ ��������� �������� � ��������� ��������� ������� � ����������.

//WebHost: ������ IWebHostBuilder, ������� ��������� ��������� ��������� ��������� �������.


// __________ sheet __________ 

//Dependencies: ��� ����������� � ������ ������ � ����������, ����� ������ �����������

//Properties: ����, ������� �������� ��������� ��������� �������. � ���������, � ����� launchSettings.json ������� ��������� ������� �������, ��������, ������, �� ������� ����� ����������� ����������.

//appsettings.json: ���� ������������ ������� � ������� json

//appsettings.Development.json: ������ ����� ������������ ����������, ������� ������������ � �������� ����������

//Program.cs: ������� ���� ����������, � �������� � ���������� ��� ����������. ��� ����� ����� ����������� � ��������� ���-����������