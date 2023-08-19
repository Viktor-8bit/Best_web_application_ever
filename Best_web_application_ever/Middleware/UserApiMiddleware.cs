using Best_web_application_ever.Services;
using System.Linq;
using System.Text.RegularExpressions;

using Best_web_application_ever.Model.Data;


namespace Best_web_application_ever.Middleware
{
    public class UserApiMiddleware
    {

        private ITimeService timeService;
        private ICountService countService;
        private readonly RequestDelegate next;

        //Класс middleware должен иметь конструктор, который принимает параметр типа RequestDelegate.
        //Через этот параметр можно получить ссылку на тот делегат запроса, который стоит следующим в конвейере обработки запроса.

        public UserApiMiddleware(RequestDelegate next, ITimeService timeService, ICountService countService)
        {
            this.next = next;
            this.timeService = timeService;
            this.countService = countService;  
        }

        // Также в классе должен быть определен метод, который должен называться либо Invoke, либо InvokeAsync.
        // Причем этот метод должен возвращать объект Task и принимать в качестве параметра контекст запроса - объект HttpContext.
        // Данный метод собственно и будет обрабатывать запрос.

        public async Task InvokeAsync(HttpContext context, ApplicationContext db)
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
                //await GetAllPeople(response);
                var persons = db.Persons.ToList();
                await response.WriteAsJsonAsync(persons);
            }
            else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "GET")
            {
                // получаем id из адреса url
                int? id = Convert.ToInt32(path.Value?.Split("/")[3]);
                
                //await GetPerson(id, response);
                
                // получаем пользователя по id
                Person? user = db.Persons.FirstOrDefault<Person>((u) => u.Id == id);
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
            else if (path == "/api/users" && request.Method == "POST" && context.User.Identity.IsAuthenticated)
            {
                //await CreatePerson(response, request);
                try
                {
                    // получаем данные пользователя
                    var user = await request.ReadFromJsonAsync<Person>();
                    if (user != null)
                    {
                        // устанавливаем id для нового пользователя
                        // добавляем пользователя в список
                        db.Persons.Add(user);
                        db.SaveChanges();
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
            else if (path == "/api/users" && request.Method == "PUT" && context.User.Identity.IsAuthenticated)
            {
                //await UpdatePerson(response, request);
                try
                {
                    // получаем данные пользователя
                    Person? userData = await request.ReadFromJsonAsync<Person>();
                    if (userData != null)
                    {
                        // получаем пользователя по id
                        var user = db.Persons.FirstOrDefault(u => u.Id == userData.Id);
                        // если пользователь найден, изменяем его данные и отправляем обратно клиенту
                        if (user != null)
                        {
                            user.Age = userData.Age;
                            user.Name = userData.Name;
                            db.SaveChanges();
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
            else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "DELETE" && context.User.Identity.IsAuthenticated)
            {
                int? id = Convert.ToInt32(path.Value?.Split("/")[3]);
                //await DeletePerson(id, response);
                // получаем пользователя по id
                Person? user = db.Persons.FirstOrDefault((u) => u.Id == id);
                // если пользователь найден, удаляем его
                if (user != null)
                {
                    db.Persons.Remove(user);
                    db.SaveChanges();
                    await response.WriteAsJsonAsync(user);
                }
                // если не найден, отправляем статусный код и сообщение об ошибке
                else
                {
                    response.StatusCode = 404;
                    await response.WriteAsJsonAsync(new { message = "Пользователь не найден" });
                }
            }
            else if (path == "/api/Time" && request.Method == "GET")
            {
                await response.WriteAsync($"{this.timeService.GetTime()}");
            }
            else if (path == "/api/Bacground" && request.Method == "GET")
            {
                await response.WriteAsync($"{countService.Count}");
            }
            else
            {
                await next.Invoke(context);
            }

        }

    }
}
