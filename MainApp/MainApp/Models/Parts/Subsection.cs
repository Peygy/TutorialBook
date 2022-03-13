namespace MainApp.Models
{
    public class Subsection
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int SectionId { get; set; }
        public Section Section { get; set; }
    }
}
