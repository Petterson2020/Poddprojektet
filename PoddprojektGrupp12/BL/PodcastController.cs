using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using Models;

namespace BL
{
    public class PodcastController : IPodcastController
    {
        private readonly IPodcastRepository _podcastRepository;
        private readonly IRssReader _rssReader;

        public PodcastController()
        {
            _podcastRepository = new PodcastRepository();
            _rssReader = new RssReader();
        }

        public async Task AddPodcastAsync(string url, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL får inte vara tomt!");
            }

            // 1. Låter din RssReader i DAL hämta podden och alla avsnitt från nätet
            Podcast newPodcast = await _rssReader.GetPodcastAsync(url, categoryName);

            // 2. Sätter ett unikt ID för databasen
            newPodcast.Id = Guid.NewGuid().ToString();

            // 3. Sparar den kompletta podden i databasen via Repository
            await _podcastRepository.CreateAsync(newPodcast);
        }

        public async Task<List<Podcast>> GetAllPodcastsAsync()
        {
            return await _podcastRepository.GetAllAsync();
        }

        // NY METOD: Används av "Hämta"-knappen för att förhandsgranska utan att spara
        public async Task<Podcast> GetPodcastPreviewAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL får inte vara tomt!");
            }

            // Använder RssReader för att bara hämta ner datan (utan att spara i databasen)
            return await _rssReader.GetPodcastAsync(url, "Förhandsgranskning");
        }
    }
}