using Microsoft.EntityFrameworkCore;
using MainApp.Models;

namespace MainApp.Services
{
    public class AuthService
    {
        // Data context for users and crew
        private UserContext data;
        // Logger for exceptions
        private ILogger<AuthService> logger;
        private CookieService cookieService;
        private HttpContext context;

        public AuthService(UserContext _db, ILogger<AuthService> _logger, HttpContext _context)
        {
            data = _db;
            logger = _logger;
            context = _context;
        }



        // Checking the user for uniqueness in the database
        public bool AvailabilityCheck(string userLogin)
        {
            try
            {
                if (!data.Users.Any(u => u.Login == userLogin)) return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return false;
        }



        public async Task AddUserAsync(string userLogin, string userPassword)
        {
            try
            {
                cookieService = new CookieService(context);

                string newPassword = HashService.HashPassword(userPassword);
                var user = new User { Login = userLogin, Password = newPassword };
                await cookieService.CookieAuthenticateAsync(userLogin, "user");

                await data.Users.AddAsync(user);
                await data.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }



        public async Task<bool> UserAuthenticationAsync(string userLogin, string userPassword)
        {
            try
            {
                if (data.Users.Any(u => u.Login == userLogin))
                {
                    var userDb = await data.Users.FirstOrDefaultAsync(u => u.Login == userLogin);
                    if (HashService.VerifyHashedPassword(userDb.Password, userPassword)) return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return false;
        }

        public async Task UserAuthorizationAsync(string userLogin, bool remember)
        {
            cookieService = new CookieService(context);

            if (remember == true)
            {
                await cookieService.CookieAuthenticateAsync(userLogin, "user");
            }
            else
            {
                cookieService.SessionAuthenticateAsync(userLogin, "user");
            }
        }


        
        public async Task<bool> AdmAuthenticationAsync(string admLogin, string admPassword)
        {
            try
            {
                cookieService = new CookieService(context);

                if (data.Crew.Any(u => u.Login == admLogin && u.Role == "admin"))
                {
                    var admDb = await data.Crew.FirstOrDefaultAsync(u => u.Login == admLogin);
                    if (HashService.VerifyHashedPassword(admDb.Password, admPassword)) 
                    {
                        await cookieService.CookieAuthenticateAsync(admLogin, "admin");
                        return true; 
                    }                 
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return false;
        }



        public async Task<bool> EdAuthenticationAsync(string edLogin, string edPassword)
        {
            try
            {
                cookieService = new CookieService(context);

                if (data.Crew.Any(u => u.Login == edLogin && u.Role == "editor"))
                {
                    var edDb = await data.Crew.FirstOrDefaultAsync(u => u.Login == edLogin);
                    if (HashService.VerifyHashedPassword(edDb.Password, edPassword))
                    {
                        await cookieService.CookieAuthenticateAsync(edLogin, "editor");
                        return true;
                    }
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
