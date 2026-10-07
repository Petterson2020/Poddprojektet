using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks; // Lade till denna för Task
using DAL;
using Models;

namespace BL
{
    public class PodcastController : IPodcastController // Ändrade till public
    {
        private readonly IPodcastRepository _podcastRepository;

        public PodcastController()
        {
            _podcastRepository = new PodcastRepository();
        }

        public async Task AddPodcastAsync(string url, string categoryName)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("URL får inte vara tomt!");
            }

            Podcast newPodcast = new Podcast
            {
                Id = Guid.NewGuid().ToString(), // Ändrade från NewGuide till NewGuid
                Title = url,
                Url = url,
                Category = categoryName
            };

            // Ändrade till CreateAsync som metoden heter i ditt Repository
            await _podcastRepository.CreateAsync(newPodcast);
        }

        public async Task<List<Podcast>> GetAllPodcastsAsync()
        {
            return await _podcastRepository.GetAllAsync();
        }
    }
}