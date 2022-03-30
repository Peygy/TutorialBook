using Microsoft.EntityFrameworkCore;

namespace MainApp.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;

        public UserContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
