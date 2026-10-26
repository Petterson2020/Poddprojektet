using Models;

namespace DAL
{
    public interface IRssReader
    {
        Task<Podcast> GetPodcastAsync(string url);
    }
}