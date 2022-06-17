using MainApp.Tests.Models.Crew;

namespace MainApp.Tests.Models.Repositories
{
    // Interface with method imitations for controller testing
    public interface ICrewRepository
    {
        // 
        public IEnumerable<User> GetUsers();
        public IEnumerable<Admin> GetAdmins();
        public bool AddNewAdmin(Admin admin);
        public User RemoveUser(int id);
        public Admin RemoveAdmin(int id);
    }
}
