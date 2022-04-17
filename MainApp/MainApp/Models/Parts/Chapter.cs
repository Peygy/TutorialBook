using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    // Section -> Subsection -> CHAPTER -> Subchapter -> Post

    // Model for Chapter
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано название")]
        public string? Title { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int SubsectionId { get; set; }
        public Subsection? Subsection { get; set; }
    }
}
