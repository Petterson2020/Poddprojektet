using DAL;
using Models;

namespace BL
{
    public class PodcastController : IPodcastController
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IRssReader _rssReader;

        public PodcastController() : this(new PodcastRepository(), new RssReader()) { }

        public PodcastController(IPodcastRepository podcastRepository, IRssReader rssReader)
        {
            _podcastRepository = podcastRepository;
            _rssReader = rssReader;
        }

        public async Task AddPodcastAsync(string url, string categoryName, string customTitle = "")
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL får inte vara tom.");
            }

            Podcast podcast = await _rssReader.GetPodcastAsync(url.Trim());
            podcast.Id = Guid.NewGuid().ToString();
            podcast.Category = categoryName;

            if (!string.IsNullOrWhiteSpace(customTitle))
            {
                podcast.Title = customTitle.Trim();
            }

            await _podcastRepository.CreateAsync(podcast);
        }

        public Task<List<Podcast>> GetAllPodcastsAsync() => _podcastRepository.GetAllAsync();

        public Task<Podcast> GetPodcastPreviewAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL får inte vara tom.");
            }

            return _rssReader.GetPodcastAsync(url.Trim());
        }

        public async Task DeletePodcastAsync(string id)
        {
            await _podcastRepository.DeleteAsync(id);
        }

        public async Task UpdatePodcastAsync(string id, string newTitle, string newCategory)
        {
            Podcast podcast = await _podcastRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Kunde inte hitta podden som skulle ändras.");

            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                podcast.Title = newTitle.Trim();
            }

            if (!string.IsNullOrWhiteSpace(newCategory))
            {
                podcast.Category = newCategory;
            }

            await _podcastRepository.UpdateAsync(podcast);
        }
    }
}