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
        private HttpContext context;
        public CookieService(HttpContext _context)
        {
            context = _context;
        }


        public async Task AuthenticateAsync(string login, string role)
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

        public async Task LogoutAsync()
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public User GetUserCookie()
        {
            var login = context.User.FindFirst(ClaimsIdentity.DefaultNameClaimType);
            var role = context.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType);

            if (login != null && role != null)
            {
                return new User { Login = login!.Value, Role = role!.Value };
            }

            return null;
        }

        public string Role() => context.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType).Value;
    }
}
