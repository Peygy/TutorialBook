using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    // SECTION -> Subsection -> Chapter -> Subchapter -> Post

    // Model for Section
    public class Section
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано название")]
        public string? Title { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
