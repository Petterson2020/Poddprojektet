using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using MongoDB.Driver;

namespace DAL
{
    public class PodcastRepository : IPodcastRepository
    {
        // Hämtas först när den används, så att appen inte kraschar vid start om konfigurationen saknas
        private static IMongoCollection<Podcast> Podcasts =>
            MongoContext.Database.GetCollection<Podcast>("Podcasts");

        public Task CreateAsync(Podcast podcast) =>
            MongoContext.RunInTransactionAsync(session => Podcasts.InsertOneAsync(session, podcast));

        public async Task<List<Podcast>> GetAllAsync() =>
            await Podcasts.Find(_ => true).ToListAsync();

        public async Task<Podcast?> GetByIdAsync(string id) =>
            await Podcasts.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<long> CountByCategoryAsync(string categoryName) =>
            await Podcasts.CountDocumentsAsync(p => p.Category == categoryName);

        public Task UpdateAsync(Podcast podcast) =>
            MongoContext.RunInTransactionAsync(session =>
                Podcasts.ReplaceOneAsync(session, p => p.Id == podcast.Id, podcast));

        public Task DeleteAsync(string id) =>
            MongoContext.RunInTransactionAsync(session =>
                Podcasts.DeleteOneAsync(session, p => p.Id == id));
    }
}