using Models;

namespace BL
{
    public interface IPodcastController
    {
        Task AddPodcastAsync(string url, string categoryName, string customTitle = "");
        Task<List<Podcast>> GetAllPodcastsAsync();

        Task<Podcast> GetPodcastPreviewAsync(string url);

        Task DeletePodcastAsync(string id);
        Task UpdatePodcastAsync(string id, string newTitle, string newCategory);
    }
}