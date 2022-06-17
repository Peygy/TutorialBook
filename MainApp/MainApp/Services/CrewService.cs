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



        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                return await data.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }

        public async Task<IEnumerable<Admin>> GetAdminsAsync()
        {
            try
            {
                return await data.Crew.Where(a => a.Role == "admin").ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }



        public async Task<bool> AddAdminAsync(Admin admin)
        {
            try
            {
                if (!data.Crew.Any(a => a.Login == admin.Login))
                {
                    string newPassword = HashService.HashPassword(admin.Password);
                    var newAdmin = new Admin { Login = admin.Login, Password = newPassword };
                    await data.Crew.AddAsync(newAdmin);
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



        public async Task<User> RemoveUserAsync(int id)
        {
            try
            {
                var user = await data.Users.FirstOrDefaultAsync(u => u.Id == id);
                data.Users.Remove(user);
                await data.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }

        public async Task<Admin> RemoveAdminAsync(int id)
        {
            try
            {
                var admin = await data.Crew.FirstOrDefaultAsync(u => u.Id == id);
                data.Crew.Remove(admin);
                await data.SaveChangesAsync();
                return admin;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return null;
        }
    }
}
