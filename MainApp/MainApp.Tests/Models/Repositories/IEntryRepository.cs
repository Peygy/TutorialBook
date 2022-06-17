using MainApp.Tests.Models.Crew;

namespace MainApp.Tests.Models.Repositories
{
    // Interface with method imitations for controller testing
    public interface IEntryRepository
    {
        // Auth methods
        public bool AvailabilityCheck(string userLogin);
        public void AddUser(string userLogin, string userPassword);
        public bool UserAuthentication(string userLogin, string userPassword);
        public void UserAuthorization(string userLogin, bool remember);
        public bool AdmAuthentication(string admLogin, string admPassword);
        public bool EdAuthentication(string edLogin, string edPassword);


        // Cookie methods
        public User GetUserInfo();
        public void Logout();
    }
}
