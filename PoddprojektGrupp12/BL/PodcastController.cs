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

        public async Task DeletePodcastAsync(string title)
        {
            // 1. Hämta alla poddar
            var allPodcasts = await _podcastRepository.GetAllAsync();

            // 2. Hitta podden med rätt titel
            var podcastToDelete = System.Linq.Enumerable.FirstOrDefault(allPodcasts, p => p.Title == title);

            if (podcastToDelete != null)
            {
                // 3. Skicka ID:t till databasen istället för titeln
                await _podcastRepository.DeleteAsync(podcastToDelete.Id);
            }
            else
            {
                throw new Exception("Kunde inte hitta podden i databasen.");
            }
        }

        public async Task UpdatePodcastAsync(string oldTitle, string newTitle, string newCategory)
        {
            // 1. Hämta alla poddar för att hitta rätt
            var allPodcasts = await _podcastRepository.GetAllAsync();
            var podcastToUpdate = System.Linq.Enumerable.FirstOrDefault(allPodcasts, p => p.Title == oldTitle);

            if (podcastToUpdate != null)
            {
                // 2. Ändra titeln om användaren skrivit in en ny
                if (!string.IsNullOrWhiteSpace(newTitle))
                {
                    podcastToUpdate.Title = newTitle;
                }

                // 3. Ändra kategorin om användaren valt en ny
                if (!string.IsNullOrWhiteSpace(newCategory))
                {
                    podcastToUpdate.Category = newCategory;
                }

                // 4. Spara ändringarna till databasen
                await _podcastRepository.UpdateAsync(podcastToUpdate);
            }
            else
            {
                throw new Exception("Kunde inte hitta podden som skulle ändras.");
            }
        }
    }
}