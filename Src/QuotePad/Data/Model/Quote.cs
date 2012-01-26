namespace QuotePad.Model
{
    public class Quote
    {
        public int Id { get; set; }
        public string RtfText { get; set; }
        public bool IsFavorite { get; set; }
        public int AuthorId { get; set; }
        public int ThemeId { get; set; }
    }
}
