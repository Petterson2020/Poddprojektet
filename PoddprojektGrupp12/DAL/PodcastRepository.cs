using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Models;

namespace DAL
{
    public class PodcastRepository : IPodcastRepository
    {
        // Variabel som håller referensen till vår specifika samling i databasen
        private readonly IMongoCollection<Podcast> _podcasts;

        public PodcastRepository()
        {
            string connectionString = "mongodb+srv://simonpetterson20_db_user:Petterson20@orumongodb.fud0i2g.mongodb.net/?appName=OruMongoDB";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("PoddAppDB");

            // Kopplar variabeln till samlingen "Podcasts" i databasen
            _podcasts = database.GetCollection<Podcast>("Podcasts");
        }

        public async Task CreateAsync(Podcast podcast)
        {
            // Lägger till en ny podd i databasen
            await _podcasts.InsertOneAsync(podcast);
        }

        public async Task<List<Podcast>> GetAllAsync()
        {
            // Find(_ => true) betyder "hämta alla dokument utan filter"
            return await _podcasts.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(Podcast podcast)
        {
            // Ersätter den befintliga podden i databasen med den uppdaterade versionen
            await _podcasts.ReplaceOneAsync(p => p.Id == podcast.Id, podcast);
        }

        public async Task DeleteAsync(string id)
        {
            // Raderar den podd vars Id matchar det vi skickar in
            await _podcasts.DeleteOneAsync(p => p.Id == id);
        }
    }
}