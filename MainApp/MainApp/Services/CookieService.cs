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
        public async Task AuthenticateAsync(string login, string role, HttpContext context)
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

        public async Task LogoutAsync(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public User GetUserCookie(HttpContext context)
        {
            var login = context.User.FindFirst(ClaimsIdentity.DefaultNameClaimType);
            var role = context.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType);

            if (login != null && role != null)
            {
                return new User { Login = login!.Value, Role = role!.Value };
            }

            return null;
        }
    }
}
