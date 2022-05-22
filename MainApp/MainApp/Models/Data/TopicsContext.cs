using Microsoft.EntityFrameworkCore;

namespace MainApp.Models
{
    // Database context for interaction between
    // the parts tables in the database and the application
    public class TopicsContext : DbContext
    {
        // Section -> Subsection -> Chapter -> Subchapter -> Post

        public DbSet<GeneralPart> Sections { get; set; } = null!;
        public DbSet<GeneralPart> Subsections { get; set; } = null!;
        public DbSet<GeneralPart> Chapters { get; set; } = null!;
        public DbSet<GeneralPart> Subchapters { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        

        public TopicsContext(DbContextOptions<TopicsContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
