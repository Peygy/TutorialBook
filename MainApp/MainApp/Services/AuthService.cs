using Microsoft.EntityFrameworkCore;
using MainApp.Models;

namespace MainApp.Services
{
    public class AuthService
    {
        private UserContext userData;
        private ILogger<AuthService> logger;

        // User register
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

        // User login
        public async Task<bool> UserAuthenticationAsync(string userLogin, string userPassword)
        {
            try
            {
                if (userData.Users.Any(u => u.Login == userLogin))
                {
                    var userDb = await userData.Users.FirstOrDefaultAsync(u => u.Login == userLogin);
                    if (HashService.VerifyHashedPassword(userDb!.Password, userPassword)) { return true; }
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return false;
        }

        public async Task UserAuthorizationAsync(string userLogin, bool remember, HttpContext context)
        {
            CookieService cookieService = new CookieService();
            
            if (remember == true)
            {
                await cookieService.AuthenticateAsync(userLogin, "user", context);
            }
        }


        // Admin services
        public async Task<bool> AdmAuthenticationAsync(string admLogin, string admPassword, HttpContext context)
        {
            try
            {
                CookieService cookieService = new CookieService();

                if (userData.Crew.Any(u => u.Login == admLogin))
                {
                    var admDb = await userData.Crew.FirstOrDefaultAsync(u => u.Login == admLogin);
                    if (HashService.VerifyHashedPassword(admDb!.Password, admPassword)) 
                    {
                        await cookieService.AuthenticateAsync(string.Empty, "admin", context);
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


        // Editor services
        public async Task<bool> EdAuthenticationAsync(string edLogin, string edPassword, HttpContext context)
        {
            try
            {
                CookieService cookieService = new CookieService();

                if (userData.Crew.Any(u => u.Login == edLogin))
                {
                    var edDb = await userData.Crew.FirstOrDefaultAsync(u => u.Login == edLogin);
                    if (HashService.VerifyHashedPassword(edDb!.Password, edPassword))
                    {
                        await cookieService.AuthenticateAsync(string.Empty, "editor", context);
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
