using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace BL
{
    public interface IPodcastController
    {
        Task AddPodcastAsync(string url, string categoryName);

        Task <List<Podcast>> GetAllPodcastsAsync();
    }
}
