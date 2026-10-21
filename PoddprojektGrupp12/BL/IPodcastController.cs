using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace BL
{
    public interface IPodcastController
    {
        Task AddPodcastAsync(string url, string categoryName, string customTitle = "");
        Task<List<Podcast>> GetAllPodcastsAsync();

        // Hämtar en podd från nätet utan att spara den (för Hämta-knappen)
        Task<Podcast> GetPodcastPreviewAsync(string url);

        Task DeletePodcastAsync(string id);
        Task UpdatePodcastAsync(string id, string newTitle, string newCategory);
    }
}