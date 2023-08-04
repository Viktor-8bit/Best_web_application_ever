using Best_web_application_ever;
using System.Text.RegularExpressions;



var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();


app.UseMiddleware<UserApiMiddleware>();

// добавляем middleware
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

// получение всех пользователей




// idk wtf sheet

//Run(): запускает приложение

//RunAsync(): асинхронно запускает приложение

//Start(): запускает приложение

//StartAsync(): запускает приложение

//StopAsync(): останавливает приложение




// __________ also sheet __________ 

//  в классе WebApplicationBuilder определены следующие свойства:

//Configuration: представляет объект ConfigurationManager, который применяется для добавления конфигурации к приложению.

//Environment: предоставляет информацию об окружении, в котором запущено приложение.

//Host: объект IHostBuilder, который применяется для настройки хоста.

//Logging: позволяет определить настройки логгирования в приложении.

//Services: представляет коллекцию сервисов и позволяет добавлять сервисы в приложение.

//WebHost: объект IWebHostBuilder, который позволяет настроить отдельные настройки сервера.


// __________ sheet __________ 

//Dependencies: все добавленные в проект пакеты и библиотеки, иначе говоря зависимости

//Properties: узел, который содержит некоторые настройки проекта. В частности, в файле launchSettings.json описаны настройки запуска проекта, например, адреса, по которым будет запускаться приложение.

//appsettings.json: файл конфигурации проекта в формате json

//appsettings.Development.json: версия файла конфигурации приложения, которая используется в процессе разработки

//Program.cs: главный файл приложения, с которого и начинается его выполнение. Код этого файла настраивает и запускает веб-приложение