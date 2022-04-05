using Microsoft.EntityFrameworkCore;
using MainApp.Models;

namespace MainApp.Services
{
    public class AuthService
    {
        private UserContext userData;

        // User register
        public async Task<bool> AvailabilityCheck(string userLogin)
        {
            try
            {
                if (!await userData.Users.AnyAsync(u => u.Login == userLogin))
                {
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            return false;
        }

        public async Task<bool> AddUser(User user, HttpContext context)
        {
            CookieService cookieService = new CookieService();

            string newPassword = HashService.HashPassword(user.Password);
            user = new User { Login = user.Login, Password = newPassword, Role = "user" };
            await cookieService.Authenticate(user, context);

            try
            {
                await userData.Users.AddAsync(user);
                await userData.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            return false;
        }

        // User login
        public async Task<bool> UserAuthentication(User user)
        {
            try
            {
                if (await userData.Users.AnyAsync(u => u.Login == user.Login))
                {
                    var userDb = await userData.Users.FirstOrDefaultAsync(u => u.Login == user.Login);
                    if (HashService.VerifyHashedPassword(userDb!.Password, user.Password)) { return true; }
                    return false;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            return false;
        }

        public async Task<bool> UserAuthorization(User user, bool remember, HttpContext context)
        {
            CookieService cookieService = new CookieService();

            string newPassword = HashService.HashPassword(user.Password);
            
            if (remember == true)
            {
                user = new User { Login = user.Login, Password = newPassword, Role = "user" };
                await cookieService.Authenticate(user, context);
            }

            try
            {
                var userUpd = await userData.Users.FirstOrDefaultAsync(u => u.Login == user.Login);
                userUpd!.Password = newPassword;

                await userData.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            return false;
        }


        // Admin services
        public async Task<bool> AdmAuthentication(Admin adm)
        {
            try
            {
                if (await userData.Users.AnyAsync(u => u.Login == adm.Login))
                {
                    var admDb = await userData.Users.FirstOrDefaultAsync(u => u.Login == adm.Login);
                    if (HashService.VerifyHashedPassword(admDb!.Password, adm.Password)) { return true; }
                    return false;
                }
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            return false;
        }

        public async Task<bool> AdmAuthorization(Admin adm)
        {
            try
            {
                string newPassword = HashService.HashPassword(adm.Password);
                var admin = await userData.Users.FirstOrDefaultAsync(a => a.Login == adm.Login);
                admin!.Password = newPassword;

                await userData.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.Clear();
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            return false;
        }
    }
}
