using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using MainApp.Models;

namespace MainApp.Services
{
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
    }
}
