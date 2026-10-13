using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using Models;

namespace DAL
{
    public class RssReader : IRssReader
    {
        public async Task<Podcast> GetPodcastAsync(string url, string categoryName)
        {
            Podcast newPodcast = new Podcast
            {
                Url = url, // Id tilldelas oftast när man sparar i databasen, men du kan lägga till Guid.NewGuid().ToString() här om du vill.
                Category = categoryName,
                Episodes = new List<Episode>()
            };

            using (HttpClient client = new HttpClient())
            {
                // DENNA RAD ÄR NY: Den förklär ditt program till en vanlig webbläsare
                client.DefaultRequestHeaders.Add("User-Agent", "PoddProjektet/1.0 (Windows NT 10.0; Win64; x64)");

                using (var stream = await client.GetStreamAsync(url))
                {
                    using (XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true }))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);

                        // Säkrare hämtning av titel
                        newPodcast.Title = feed.Title?.Text ?? "Okänd titel";

                        foreach (SyndicationItem item in feed.Items)
                        {
                            Episode episode = new Episode
                            {
                                Title = item.Title?.Text ?? "Ingen titel",
                                Description = item.Summary?.Text ?? "Ingen beskrivning",
                                PublishDate = item.PublishDate.ToString("yyyy-MM-dd")
                            };
                            newPodcast.Episodes.Add(episode);
                        }
                    }
                }
            }

            return newPodcast;
        }
    }
}