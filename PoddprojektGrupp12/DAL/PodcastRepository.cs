using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Models;

namespace DAL
{
    public class PodcastRepository : IPodcastRepository
    {
        private readonly IMongoCollection<Podcast> _podcasts;

        public PodcastRepository()
        {
            string connectionString = "mongodb+srv://simonpetterson20_db_user:Petterson20@orumongodb.fud0i2g.mongodb.net/?appName=OruMongoDB";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("PoddAppDB");

            _podcasts = database.GetCollection<Podcast>("Podcasts");
        }

        public async Task CreateAsync(Podcast podcast)
        {
            await _podcasts.InsertOneAsync(podcast);
        }

        public async Task<List<Podcast>> GetAllAsync()
        {
            return await _podcasts.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(Podcast podcast)
        {
            await _podcasts.ReplaceOneAsync(p => p.Id == podcast.Id, podcast);
        }

        // Endast EN DeleteAsync, och den raderar på Title!
        public async Task DeleteAsync(string title)
        {
            await _podcasts.DeleteOneAsync(p => p.Title == title);
        }
    }
}