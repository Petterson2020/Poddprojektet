using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Net.Http; // MÅSTE LÄGGAS TILL FÖR HTTPCLIENT
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

            Podcast newPodcast = new Podcast
            {
                Id = Guid.NewGuid().ToString(),
                Url = url,
                Category = categoryName,
                Episodes = new List<Episode>()
            };

            try
            {
                // NYTT: Använder HttpClient för att skicka med en User-Agent
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "PoddProjektet/1.0 (Windows NT 10.0; Win64; x64)");

                    using (var responseStream = await client.GetStreamAsync(url))
                    using (var reader = XmlReader.Create(responseStream))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);

                        newPodcast.Title = feed.Title?.Text ?? "Okänd titel";

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
            }
            catch (Exception ex)
            {
                throw new Exception("Kunde inte läsa RSS-flödet. Kontrollera att länken är korrekt.", ex);
            }

            await _podcastRepository.CreateAsync(newPodcast);
        }

        public async Task<List<Podcast>> GetAllPodcastsAsync()
        {
            return await _podcastRepository.GetAllAsync();
        }
    }
}