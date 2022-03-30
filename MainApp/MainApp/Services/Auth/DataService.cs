using Microsoft.EntityFrameworkCore;
using MainApp.Models;

namespace MainApp.Services.Auth
{
    public class DataService
    {
        private UserContext userData;


        public bool AvailabilityCheck(string userLogin)
        {
            try
            {
                if (!userData.Users.Any(u => u.Login == userLogin))
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

        public bool Authentication(string userLogin, string userPassword)
        {
            try
            {
                if (userData.Users.Any(u => u.Login == userLogin))
                {
                    string dbPassword = userData.Users.FirstOrDefault(u => u.Login == userLogin)!.Password;
                    if (HashService.VerifyHashedPassword(dbPassword, userPassword) == true) { return true; }
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



        public async Task<bool> AddUser(string userLogin, string userPassword, HttpContext context)
        {
            CookieService cookieService = new CookieService();

            string newPassword = HashService.HashPassword(userPassword);           
            User user = new User { Login = userLogin, Password = newPassword, Role = "user"};
            await cookieService.Authenticate(user, context);

            try
            {
                userData.Users.Add(user);
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

        public async Task<bool> Authorization(string userLogin, string userPassword, bool remember, HttpContext context)
        {
            CookieService cookieService = new CookieService();

            string newPassword = HashService.HashPassword(userPassword);
            User user = new User { Login = userLogin, Password = newPassword, Role = "user" };
            
            if (remember == true)
            {
                await cookieService.Authenticate(user, context);
            }

            try
            {
                userData.Users.Add(user);
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
