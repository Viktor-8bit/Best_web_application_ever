using Best_web_application_ever;
using System.Text.RegularExpressions;



var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();


app.UseMiddleware<UserApiMiddleware>();

// ��������� middleware
//app.UseMiddleware<TokenMiddleware>();

app.Map("/Time", appBuilder =>
{
    var time = DateTime.Now.ToShortTimeString();

    appBuilder.Run(async context => await context.Response.WriteAsync($"Time: {time}"));
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