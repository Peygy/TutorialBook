using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using MainApp.Models;

namespace MainApp.Services
{
    // Service to generate cookies when logging in or registering
    // and deleting them when logging out of the site
    public class CookieService
    {
        public HttpContext context;

        public CookieService(HttpContext _context)
        {
            context = _context;
        }



        public async Task CookieAuthenticateAsync(string login, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await context.SignInAsync(claimsPrincipal);
        }

        public void SessionAuthenticateAsync(string login, string role)
        {
            context.Session.SetString("login", login);
            context.Session.SetString("role", role);
        }



        public async Task LogoutAsync()
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Session.Clear();
        }



        public User GetUserInfo()
        {
            var loginCookie = context.User.FindFirst(ClaimsIdentity.DefaultNameClaimType); 
            var roleCookie = context.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType); 
            if (loginCookie != null && roleCookie != null)
            {
                return new User {Login = loginCookie.Value, Role = roleCookie.Value};
            }

            var loginSession = context.Session.GetString("login");
            var roleSession = context.Session.GetString("role");
            if (loginSession != null && roleSession != null)
            {
                return new User { Login = loginSession, Role = roleSession };
            }

            return new User { Role = "null" };
        }
    }
}
