using Best_web_application_ever.Model.Data;
using Best_web_application_ever.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System;
using System.Web;

namespace Best_web_application_ever.Middleware
{
    public class LoginMidleware
    {
        private readonly RequestDelegate next;
        private readonly HashService hashService;

        public LoginMidleware(RequestDelegate next, HashService hashService)
        {
            this.next = next;
            this.hashService = hashService;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationContext db)
        {
            var Responce = context.Response;
            var Request = context.Request;
            var path = Request.Path;

            if (Request.Method == "POST" && path == "/login")
            {
                Responce.ContentType = "text/html; charset=utf-8";

                var form = Request.Form;

                if (!form.ContainsKey("login") || !form.ContainsKey("password"))
                {
                    Responce.StatusCode = 415;
                    await Responce.WriteAsync("login и/или password не установлены");
                }

                string? login = form["login"];
                string? passsword = form["password"];
                passsword = hashService.getHashSha256(passsword);


                // находим пользователя 
                User? user = db.Users.FirstOrDefault(u => u.Login == login && u.HashPassword == passsword);

                // если пользователь не найден, отправляем статусный код 401
                if (user is null)
                {
                    await Responce.WriteAsync("ошибка в логине или в пароле");
                }
                else
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };

                    // создаем объект ClaimsIdentity
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

                    // установка аутентификационных куки
                    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    var cookies = Responce.Cookies;

                    if (Request.Cookies["user_name"] == null)
                    {
                        cookies.Append("user_name", user.Login);
                    }
                    else
                    {
                        cookies.Delete("user_name");
                        cookies.Append("user_name", user.Login);
                    }

                    Responce.Redirect("/");
                }
                
            }
            else if (Request.Method == "GET" && path == "/login") 
            {
                Responce.ContentType = "text/html; charset=utf-8";
                await Responce.SendFileAsync("wwwroot/login.html");
            }
            else
            {
                await next.Invoke(context);
            }

        }
    }
}
