using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Syndication;
using DAL;
using Models;

namespace BL
{
    public class PodcastController : IPodcastController
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

            // Skapa grundobjektet och initiera listan för avsnitt
            Podcast newPodcast = new Podcast
            {
                Id = Guid.NewGuid().ToString(),
                Url = url,
                Category = categoryName,
                Episodes = new List<Episode>()
            };

            try
            {
                // Ladda ner och läs RSS-flödet från internet
                using (XmlReader reader = XmlReader.Create(url))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);

                    // Hämta den riktiga titeln från podden (istället för att sätta URL som titel)
                    newPodcast.Title = feed.Title?.Text ?? "Okänd titel";

                    // Loopa igenom alla avsnitt i flödet och lägg till dem
                    foreach (SyndicationItem item in feed.Items)
                    {
                        Episode newEpisode = new Episode
                        {
                            Title = item.Title?.Text ?? "Okänt avsnitt",
                            Description = item.Summary?.Text ?? "Ingen beskrivning tillgänglig."
                        };
                        newPodcast.Episodes.Add(newEpisode);
                    }
                }
            }
            catch (Exception ex)
            {
                // Kastar ett fel till PL om nedladdningen misslyckas (t.ex. ogiltig länk)
                throw new Exception("Kunde inte läsa RSS-flödet. Kontrollera att länken är korrekt.", ex);
            }

            // Spara den kompletta podden med alla avsnitt i databasen via DAL
            await _podcastRepository.CreateAsync(newPodcast);
        }

        public async Task<List<Podcast>> GetAllPodcastsAsync()
        {
            return await _podcastRepository.GetAllAsync();
        }
    }
}