using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MainApp.Services
{
    [ViewComponent]
    public class RoleService
    {
        HttpContext context;

        public string Invoke() => context.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType).Value;
    }
}
