using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace DAL
{
    public interface IRssReader
    {
        Task<Podcast> GetPodcastAsync(string url, string categoryName);
    }
}
