using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    // Section -> Subsection -> Chapter -> SUBCHAPTER -> Post

    // Model for Subchapter
    public class Subchapter
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано название")]
        public string? Title { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int ChapterId { get; set; }
        public Chapter? Chapter { get; set; }
    }
}
