using Microsoft.EntityFrameworkCore;

namespace MainApp.Models
{
    public class TopicsContext : DbContext
    {
        public DbSet<Section> Sections { get; set; } = null!;
        public DbSet<Subsection> SubSections { get; set; } = null!;
        public DbSet<Chapter> Chapters { get; set; } = null!;
        public DbSet<Subchapter> SubChapters { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        

        public TopicsContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
