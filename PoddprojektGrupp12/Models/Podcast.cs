namespace Models
{
    public class Podcast
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<Episode> Episodes { get; set; } = new List<Episode>();
    }
}
