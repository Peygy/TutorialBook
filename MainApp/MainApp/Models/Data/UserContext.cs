using Microsoft.EntityFrameworkCore;

namespace MainApp.Models
{
    public class UserContext : DbContext
    {
        DbSet<User> Users { get; set; } = null!;

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
