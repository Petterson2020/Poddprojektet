using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Podcast
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public List<Episode> Episodes { get; set; } = new List<Episode>(); // Håller alla avsnitt
    }
}
