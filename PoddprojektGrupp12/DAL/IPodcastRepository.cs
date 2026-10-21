using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DAL
{
    public interface IPodcastRepository
    {
        Task CreateAsync(Podcast podcast);
        Task<List<Podcast>> GetAllAsync();
        Task<Podcast?> GetByIdAsync(string id);
        Task<long> CountByCategoryAsync(string categoryName);
        Task UpdateAsync(Podcast podcast);
        Task DeleteAsync(string id);
    }
}