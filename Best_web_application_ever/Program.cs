using Best_web_application_ever.Middleware;
using Best_web_application_ever.Services;
using System.Text.RegularExpressions;




var builder = WebApplication.CreateBuilder();

// AddTransient � ASP .NET - ��� �����, ������� ������������ ��� ����������� ������� � ���������� ��������� ������������ � ��������� ��������� ������.
// �� �������� �� �������� ������ ���������� ������� ������ ���, ����� �� �������������!

builder.Services.AddTransient<ShortTimeService>();
builder.Services.AddTransient<ITimeService, LongTimeService>();

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


builder.Services.AddSingleton<ICountService, SomePlusPlusService>();

builder.Services.AddHostedService<CringeCountService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();



app.UseMiddleware<UserApiMiddleware>();


// ��������� middleware � �������
// app.UseMiddleware<TokenMiddleware>();



app.Map("/Amogus", appBuilder =>
{
    appBuilder.Run(async context => {

        await context.Response.WriteAsync($"amogus ");
    
    });
});

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    
    response.ContentType = "text/html; charset=utf-8";
    await response.SendFileAsync("wwwroot/html/index.html");
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