using System.Linq;
using System.Text.RegularExpressions;

namespace Best_web_application_ever
{
    public class UserApiMiddleware
    {

        static List<Person> users = new List<Person>
        {
                new() { Id = 0, Name = "Tom", Age = 37 },
                new() { Id = 1, Name = "Bob", Age = 41 },
                new() { Id = 2, Name = "Sam", Age = 24 }
        };

        private readonly RequestDelegate next;

        //Класс middleware должен иметь конструктор, который принимает параметр типа RequestDelegate.
        //Через этот параметр можно получить ссылку на тот делегат запроса, который стоит следующим в конвейере обработки запроса.

        public UserApiMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        // Также в классе должен быть определен метод, который должен называться либо Invoke, либо InvokeAsync.
        // Причем этот метод должен возвращать объект Task и принимать в качестве параметра контекст запроса - объект HttpContext.
        // Данный метод собственно и будет обрабатывать запрос.

        public async Task InvokeAsync(HttpContext context)
        {
            var response = context.Response;
            var request = context.Request;
            var path = request.Path;
            Console.WriteLine(path);
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
                await next.Invoke(context);
            }

        }

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
    }
}
