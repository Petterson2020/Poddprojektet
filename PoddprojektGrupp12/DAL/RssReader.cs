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
        // Fix 1: Lade till '>' efter Podcast
        public async Task<Podcast> GetPodcastAsync(string url, string categoryName)
        {
            Podcast newPodcast = new Podcast
            {
                Url = url,
                Category = categoryName,
                Episodes = new List<Episode>()
            };

            using (HttpClient client = new HttpClient())
            {
                // Fix 2: Hämtar dataströmmen från nätet först
                using (var stream = await client.GetStreamAsync(url))
                {
                    // Skickar in strömmen (stream) till XmlReader
                    using (XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true }))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        newPodcast.Title = feed.Title.Text;

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

            // Fix 3: Rättstavat return innan metoden stängs
            return newPodcast;
        }
    }
}
// Fix 4: Tog bort den extra klammern här