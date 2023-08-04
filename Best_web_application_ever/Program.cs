using System.Text.RegularExpressions;

// начальные данные
List<Person> users = new List<Person>
{
    new() { Id = 0, Name = "Tom", Age = 37 },
    new() { Id = 1, Name = "Bob", Age = 41 },
    new() { Id = 2, Name = "Sam", Age = 24 }
};

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;
    //string expressionForNumber = "^/api/users/([0-9]+)$";   // если id представляет число

    // 2e752824-1657-4c7f-844b-6ec2e168e99c
    string expressionForGuid = @"^/api/users/([0-9]+)$";

    if (path == "/api/users" && request.Method == "GET")
    {
        await GetAllPeople(response);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "GET")
    {
        // получаем id из адреса url
        int? id = System.Convert.ToInt32(path.Value?.Split("/")[3]);
        await GetPerson(id, response);
    }
    else if (path == "/api/users" && request.Method == "POST")
    {
        await CreatePerson(response, request);
    }
    else if (path == "/api/users" && request.Method == "PUT")
    {
        await UpdatePerson(response, request);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "DELETE")
    {
        int? id = System.Convert.ToInt32(path.Value?.Split("/")[3]);
        await DeletePerson(id, response);
    }
    else
    {
        //if (path == "/js/from_js")
        //if (path.Value?.Split("/")[1] == "js")
        //{
        //    response.ContentType = "application/javascript";
        //    string file = path.Value?.Split("/")[2];
        //    if (file != null)
        //        await response.SendFileAsync($"/js/{file}.js");
            
        //}

        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("wwwroot/html/index.html");
    }
});

app.Run();

// получение всех пользователей
async Task GetAllPeople(HttpResponse response)
{
    await response.WriteAsJsonAsync(users);
}

// получение одного пользователя по id
async Task GetPerson(int? id, HttpResponse response)
{
    // получаем пользователя по id
    Person? user = users.FirstOrDefault((u) => u.Id == id);
    // если пользователь найден, отправляем его
    if (user != null)
        await response.WriteAsJsonAsync(user);
    // если не найден, отправляем статусный код и сообщение об ошибке
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
    }
}

async Task DeletePerson(int? id, HttpResponse response)
{
    // получаем пользователя по id
    Person? user = users.FirstOrDefault((u) => u.Id == id);
    // если пользователь найден, удаляем его
    if (user != null)
    {
        users.Remove(user);
        await response.WriteAsJsonAsync(user);
    }
    // если не найден, отправляем статусный код и сообщение об ошибке
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
    }
}

async Task CreatePerson(HttpResponse response, HttpRequest request)
{
    try
    {
        // получаем данные пользователя
        var user = await request.ReadFromJsonAsync<Person>();
        if (user != null)
        {
            // устанавливаем id для нового пользователя
            user.Id = users.Count;
            // добавляем пользователя в список
            users.Add(user);
            await response.WriteAsJsonAsync(user);
        }
        else
        {
            throw new Exception("Некорректные данные");
        }
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
    }
}

async Task UpdatePerson(HttpResponse response, HttpRequest request)
{
    try
    {
        // получаем данные пользователя
        Person? userData = await request.ReadFromJsonAsync<Person>();
        if (userData != null)
        {
            // получаем пользователя по id
            var user = users.FirstOrDefault(u => u.Id == userData.Id);
            // если пользователь найден, изменяем его данные и отправляем обратно клиенту
            if (user != null)
            {
                user.Age = userData.Age;
                user.Name = userData.Name;
                await response.WriteAsJsonAsync(user);
            }
            else
            {
                response.StatusCode = 404;
                await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
            }
        }
        else
        {
            throw new Exception("Некорректные данные");
        }
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Некорректные данные" });
    }
}



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