using Microsoft.EntityFrameworkCore;
using MainApp.Models;

namespace MainApp.Services
{
    public class CrewService
    {
        private UserContext userContext;


        public async Task<Array> GetCrewOrUsersAsync(string role)
        {
            switch (role) 
            {
                case "users":
                    {
                        var users = await userContext.Users.ToListAsync();
                        return users.ToArray();
                    }
                case "admins":
                    {
                        var admins = await userContext.Users.Where(a => a.Role == "admin").ToListAsync();
                        return admins.ToArray();
                    }
            }
            return null;
        }

        public async Task<bool> AddAdminAsync(string name, string password)
        {
            if (!userContext.Crew.Any(a => a.Login == name))
            {
                string newPassword = HashService.HashPassword(password);
                var admin = new Admin { Login = name, Password = newPassword, Role = "admin" };
                await userContext.Crew.AddAsync(admin);
                await userContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<JsonContent> RemoveCrewAsync(int id, string role)
        {
            switch (role)
            {
                case "users":
                    {
                        var user = await userContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                        userContext.Users.Remove(user);
                        await userContext.SaveChangesAsync();
                        return JsonContent.Create(user);
                    }
                case "admins":
                    {
                        var admin = await userContext.Crew.FirstOrDefaultAsync(u => u.Id == id);
                        userContext.Crew.Remove(admin);
                        await userContext.SaveChangesAsync();
                        return JsonContent.Create(admin);
                    }
            }
            return null;
        }
    }
}
