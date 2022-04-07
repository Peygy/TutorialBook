using System.ComponentModel.DataAnnotations;

namespace MainApp.Models
{
    public class Subsection
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        private const string TableName = "subsection";

        public int SectionId { get; set; }
        public Section Section { get; set; }
    }
}
