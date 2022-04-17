using Microsoft.EntityFrameworkCore;
using MainApp.Models;

namespace MainApp.Services
{
    // Service for fetching users or crew from the database and passing them to display
    public class CrewService
    {
        // Data context for users and crew
        private UserContext data;
        // Logger for exceptions
        private ILogger<CrewService> logger;
        public CrewService(UserContext _db, ILogger<CrewService> _logger)
        {
            data = _db;
            logger = _logger;
        }



        public async Task<Array> GetCrewOrUsersAsync(string role)
        {
            try
            {
                switch (role)
                {
                    case "users":
                        {
                            var users = await data.Users.ToListAsync();
                            return users.ToArray();
                        }
                    case "admins":
                        {
                            var admins = await data.Users.Where(a => a.Role == "admin").ToListAsync();
                            return admins.ToArray();
                        }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }

        public async Task<bool> AddAdminAsync(string name, string password)
        {
            try
            {
                if (!data.Crew.Any(a => a.Login == name))
                {
                    string newPassword = HashService.HashPassword(password);
                    var admin = new Admin { Login = name, Password = newPassword, Role = "admin" };
                    await data.Crew.AddAsync(admin);
                    await data.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return false;
        }

        public async Task<JsonContent> RemoveCrewAsync(int id, string role)
        {
            try
            {
                switch (role)
                {
                    case "users":
                        {
                            var user = await data.Users.FirstOrDefaultAsync(u => u.Id == id);
                            data.Users.Remove(user);
                            await data.SaveChangesAsync();
                            return JsonContent.Create(user);
                        }
                    case "admins":
                        {
                            var admin = await data.Crew.FirstOrDefaultAsync(u => u.Id == id);
                            data.Crew.Remove(admin);
                            await data.SaveChangesAsync();
                            return JsonContent.Create(admin);
                        }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }
    }
}
