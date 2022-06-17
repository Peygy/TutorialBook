using System.ComponentModel.DataAnnotations;

namespace MainApp.Tests.Models.Parts
{
    public class GeneralPart
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Table { get; set; } = "";
        public DateTime? CreatedDate { get; set; } = null;

        public int ParentId { get; set; }
        public GeneralPart? Parent { get; set; }
    }
}
