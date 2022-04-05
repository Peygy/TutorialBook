using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using MainApp.Models;

namespace MainApp.Services
{
    public class CookieService
    {
        public async Task Authenticate(User user, HttpContext context)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Password),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await context.SignInAsync(claimsPrincipal);
        }

        public async Task Logout(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
