namespace MainApp.Models
{
    // Model for GeneralPart, which is used for the convenience of entering parts into views
    public class GeneralPart
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Table { get; set; }
    }
}
