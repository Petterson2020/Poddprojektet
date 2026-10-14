using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DAL
{
    public interface ICategoryRepository
    {
        Task CreateAsync(Category category);
        Task<List<Category>> GetAllAsync();
        Task UpdateAsync(Category category);
        Task DeleteAsync(string categoryName);
    }
}