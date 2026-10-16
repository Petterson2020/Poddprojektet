using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Models;

namespace DAL
{
    public class PodcastRepository : IPodcastRepository
    {
        private readonly IMongoCollection<Podcast> _podcasts;
        private readonly MongoClient _client; // Ny: Vi sparar klienten för att kunna starta sessioner

        public PodcastRepository()
        {
            string connectionString = "mongodb+srv://simonpetterson20_db_user:Petterson20@orumongodb.fud0i2g.mongodb.net/?appName=OruMongoDB";
            _client = new MongoClient(connectionString);
            IMongoDatabase database = _client.GetDatabase("PoddAppDB");
            _podcasts = database.GetCollection<Podcast>("Podcasts");
        }

        public async Task CreateAsync(Podcast podcast)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                await _podcasts.InsertOneAsync(session, podcast);
                await session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task<List<Podcast>> GetAllAsync()
        {
            // Hämtning av data behöver inte ligga i en transaktion
            return await _podcasts.Find(_ => true).ToListAsync();
        }

        public async Task UpdateAsync(Podcast podcast)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                await _podcasts.ReplaceOneAsync(session, p => p.Id == podcast.Id, podcast);
                await session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                // Ändrad till p => p.Id == id
                await _podcasts.DeleteOneAsync(session, p => p.Id == id);
                await session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }
    }
}