using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    // Section -> SUBSECTION -> Chapter -> Subchapter -> Post

    // Model for Subsection
    public class Subsection
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано название")]
        public string? Title { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int SectionId { get; set; }
        public Section? Section { get; set; }
    }
}
