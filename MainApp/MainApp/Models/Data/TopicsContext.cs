using Microsoft.EntityFrameworkCore;

namespace MainApp.Models
{
    public class TopicsContext : DbContext
    {
        DbSet<Section> Sections { get; set; } = null!;
        DbSet<Subsection> Subsections { get; set; } = null!;
        DbSet<Chapter> Chapters { get; set; } = null!;
        DbSet<Subchapter> Subchapters { get; set; } = null!;

        public TopicsContext(DbContextOptions<TopicsContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
