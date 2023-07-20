using System.Text.RegularExpressions;

// ��������� ������
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
    //string expressionForNumber = "^/api/users/([0-9]+)$";   // ���� id ������������ �����

    // 2e752824-1657-4c7f-844b-6ec2e168e99c
    string expressionForGuid = @"^/api/users/([0-9]+)$";

    if (path == "/api/users" && request.Method == "GET")
    {
        await GetAllPeople(response);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "GET")
    {
        // �������� id �� ������ url
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

// ��������� ���� �������������
async Task GetAllPeople(HttpResponse response)
{
    await response.WriteAsJsonAsync(users);
}

// ��������� ������ ������������ �� id
async Task GetPerson(int? id, HttpResponse response)
{
    // �������� ������������ �� id
    Person? user = users.FirstOrDefault((u) => u.Id == id);
    // ���� ������������ ������, ���������� ���
    if (user != null)
        await response.WriteAsJsonAsync(user);
    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "������������ �� ������" });
    }
}

async Task DeletePerson(int? id, HttpResponse response)
{
    // �������� ������������ �� id
    Person? user = users.FirstOrDefault((u) => u.Id == id);
    // ���� ������������ ������, ������� ���
    if (user != null)
    {
        users.Remove(user);
        await response.WriteAsJsonAsync(user);
    }
    // ���� �� ������, ���������� ��������� ��� � ��������� �� ������
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = "������������ �� ������" });
    }
}

async Task CreatePerson(HttpResponse response, HttpRequest request)
{
    try
    {
        // �������� ������ ������������
        var user = await request.ReadFromJsonAsync<Person>();
        if (user != null)
        {
            // ������������� id ��� ������ ������������
            user.Id = users.Count;
            // ��������� ������������ � ������
            users.Add(user);
            await response.WriteAsJsonAsync(user);
        }
        else
        {
            throw new Exception("������������ ������");
        }
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "������������ ������" });
    }
}

async Task UpdatePerson(HttpResponse response, HttpRequest request)
{
    try
    {
        // �������� ������ ������������
        Person? userData = await request.ReadFromJsonAsync<Person>();
        if (userData != null)
        {
            // �������� ������������ �� id
            var user = users.FirstOrDefault(u => u.Id == userData.Id);
            // ���� ������������ ������, �������� ��� ������ � ���������� ������� �������
            if (user != null)
            {
                user.Age = userData.Age;
                user.Name = userData.Name;
                await response.WriteAsJsonAsync(user);
            }
            else
            {
                response.StatusCode = 404;
                await response.WriteAsJsonAsync(new { message = "������������ �� ������" });
            }
        }
        else
        {
            throw new Exception("������������ ������");
        }
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "������������ ������" });
    }
}



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