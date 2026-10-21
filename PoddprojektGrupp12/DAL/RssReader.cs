using System;
using System.IO;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using Models;

namespace DAL
{
    public class RssReader : IRssReader
    {

        private static readonly HttpClient Client = CreateClient();

        private static HttpClient CreateClient()
        {
            var client = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
            client.DefaultRequestHeaders.UserAgent.ParseAdd("PoddProjektet/1.0");
            return client;
        }

        public async Task<Podcast> GetPodcastAsync(string url)
        {
            try
            {
                using Stream stream = await Client.GetStreamAsync(url);

                // Parsningen är synkron, så den körs på en bakgrundstråd för att inte frysa fönstret
                return await Task.Run(() => ParseFeed(stream, url));
            }
            catch (HttpRequestException ex)
            {
                throw new FeedException("Kunde inte hämta flödet. Kontrollera adressen och din internetanslutning.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new FeedException("Det tog för lång tid att hämta flödet.", ex);
            }
            catch (XmlException ex)
            {
                throw new FeedException("Adressen innehåller inget giltigt RSS-flöde.", ex);
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is UriFormatException)
            {
                throw new FeedException("Adressen är inte giltig.", ex);
            }
        }

        private static Podcast ParseFeed(Stream stream, string url)
        {
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore };
            using XmlReader reader = XmlReader.Create(stream, settings);
            SyndicationFeed feed = SyndicationFeed.Load(reader);

            var podcast = new Podcast
            {
                Url = url,
                Title = feed.Title?.Text ?? "Okänd titel"
            };

            foreach (SyndicationItem item in feed.Items)
            {
                string description = item.Summary?.Text
                    ?? (item.Content as TextSyndicationContent)?.Text
                    ?? "Ingen beskrivning";

                podcast.Episodes.Add(new Episode
                {
                    Title = item.Title?.Text ?? "Ingen titel",
                    Description = description,
                    PublishDate = item.PublishDate == DateTimeOffset.MinValue
                        ? "Okänt datum"
                        : item.PublishDate.ToString("yyyy-MM-dd")
                });
            }

            return podcast;
        }
    }
}