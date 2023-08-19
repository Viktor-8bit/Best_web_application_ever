using Best_web_application_ever.Model.Data;

using Best_web_application_ever.Services;

namespace Best_web_application_ever.Middleware
{
    public class RegisterMidleware
    {
        private readonly RequestDelegate next;
        private readonly HashService hashService;
        public RegisterMidleware(RequestDelegate next, HashService hashService)
        {
            this.next = next;
            this.hashService = hashService;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationContext db)
        {
            var Responce = context.Response;
            var Request = context.Request;
            var path = Request.Path;

            if (Request.Method == "POST" && Request.Path == "/register")
            {
                var form = Request.Form;

                if (!form.ContainsKey("login") || !form.ContainsKey("password"))
                {
                    Responce.StatusCode = 415;
                    await Responce.WriteAsync("login и/или password не установлены");
                }
                
                string? login = form["login"];


                if ( db.Users.FirstOrDefault(x => x.Login == login) is not null )
                {
                    Responce.StatusCode = 415;
                    await Responce.WriteAsync("такой login уже есть в базе (☞ﾟヮﾟ)☞");
                }

                string? passsword = hashService.getHashSha256(form["password"]);

                db.Users.Add(new User { Login = login, HashPassword = passsword });
                db.SaveChanges();
                Responce.Redirect("/login");
                await Responce.WriteAsync("ok");
            }
            else if (Request.Method == "GET" && Request.Path == "/register")
            {
                Responce.ContentType = "text/html; charset=utf-8";
                await Responce.SendFileAsync("wwwroot/register.html");
            }
            else
            {
                await next.Invoke(context);
            }

        }
    }
}
