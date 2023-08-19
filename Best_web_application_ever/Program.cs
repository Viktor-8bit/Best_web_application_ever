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

// AddTransient в ASP .NET - это метод, который используется для регистрации сервиса в контейнере внедрения зависимостей с временным жизненным циклом.
// Он отвечает за создание нового экземпляра сервиса каждый раз, когда он запрашивается!

builder.Services.AddTransient<ShortTimeService>();
builder.Services.AddTransient<ITimeService, LongTimeService>();
builder.Services.AddTransient<HashService>();

// Transient: при каждом обращении к сервису создается новый объект сервиса.
// В течение одного запроса может быть несколько обращений к сервису, соответственно при каждом обращении будет создаваться новый объект.
// Подобная модель жизненного цикла наиболее подходит для легковесных сервисов, которые не хранят данных о состоянии

// Scoped: для каждого запроса создается свой объект сервиса.
// То есть если в течение одного запроса есть несколько обращений к одному сервису, то при всех этих обращениях будет использоваться один и тот же объект сервиса.

// Singleton: объект сервиса создается при первом обращении к нему, все последующие запросы используют один
// и тот же ранее созданный объект сервиса

// можно ещё так
// builder.Services.AddTransient<ITimeService, LongTimeService>();
// и тогда обращаемся так app.Services.GetService<ITimeService>();


// добавим сервисы аутентификацию (представляет процесс определения пользователя)
builder.Services.AddAuthentication("Cookies")
    .AddCookie( options => 
    { 
        options.LoginPath = "/login"; 
        options.LogoutPath = "/logout";
    } );

// добавляет авторизацию (представляет процесс определения, имеет ли пользователь право доступа к некоторому ресурсу) 
builder.Services.AddAuthorization();

builder.Services.AddSingleton<ICountService, SomePlusPlusService>();

builder.Services.AddHostedService<CringeCountService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();




// добавление middleware аутентификации 
app.UseAuthentication();   

// добавление middleware авторизации 
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