using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Episode
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
    }
}
