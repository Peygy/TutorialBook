using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    // Model for posts content
    public class ContentModel
    {
        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }
    }
}
