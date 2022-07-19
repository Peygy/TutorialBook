using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    // Section -> Subsection -> Chapter -> Subchapter -> Post

    // Model for Part, which is used to merge all parts into one
    public class GeneralPart
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Table { get; set; } = "";
        public DateTime? CreatedDate { get; set; }

        public int? ParentId { get; set; }
        public string? ParentTable { get; set; }
    }
}
