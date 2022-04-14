using Microsoft.EntityFrameworkCore;
using MainApp.Models;

namespace MainApp.Services
{
    public class AuthService
    {
        // Data context for users and crew
        private UserContext userData;
        // Logger for exceptions
        private ILogger<AuthService> logger;


        // Checking the user for uniqueness in the database
        public bool AvailabilityCheck(string userLogin)
        {
            try
            {
                if (!userData.Users.Any(u => u.Login == userLogin))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return false;
        }

        // Adding a user to the database during registration and storing it in cookies
        public async Task<bool> AddUserAsync(string userLogin, string userPassword, HttpContext context)
        {
            CookieService cookieService = new CookieService();

            string newPassword = HashService.HashPassword(userPassword);
            var user = new User { Login = userLogin, Password = newPassword, Role = "user" };
            await cookieService.AuthenticateAsync(userLogin, "user", context);

            try
            {
                await userData.Users.AddAsync(user);
                await userData.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return false;
        }


        // User authentication in the system
        public async Task<bool> UserAuthenticationAsync(string userLogin, string userPassword)
        {
            try
            {
                if (userData.Users.Any(u => u.Login == userLogin))
                {
                    var userDb = await userData.Users.FirstOrDefaultAsync(u => u.Login == userLogin);
                    if (HashService.VerifyHashedPassword(userDb.Password, userPassword)) { return true; }
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return false;
        }

        // User authorization in the system
        public async Task UserAuthorizationAsync(string userLogin, bool remember, HttpContext context)
        {
            CookieService cookieService = new CookieService();
            
            if (remember == true)
            {
                await cookieService.AuthenticateAsync(userLogin, "user", context);
            }
        }


        // Admin authentication in the system
        public async Task<bool> AdmAuthenticationAsync(string admLogin, string admPassword, HttpContext context)
        {
            try
            {
                CookieService cookieService = new CookieService();

                if (userData.Crew.Any(u => u.Login == admLogin))
                {
                    var admDb = await userData.Crew.FirstOrDefaultAsync(u => u.Login == admLogin);
                    if (HashService.VerifyHashedPassword(admDb.Password, admPassword)) 
                    {
                        await cookieService.AuthenticateAsync(admLogin, "admin", context);
                        return true; 
                    }
                    return false;                   
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return false;
        }


        // Editor authentication in the system
        public async Task<bool> EdAuthenticationAsync(string edLogin, string edPassword, HttpContext context)
        {
            try
            {
                CookieService cookieService = new CookieService();

                if (userData.Crew.Any(u => u.Login == edLogin))
                {
                    var edDb = await userData.Crew.FirstOrDefaultAsync(u => u.Login == edLogin);
                    if (HashService.VerifyHashedPassword(edDb.Password, edPassword))
                    {
                        await cookieService.AuthenticateAsync(edLogin, "editor", context);
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return false;
        }
    }
}
