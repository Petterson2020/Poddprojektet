using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace DAL
{
    public interface ICategoryRepository
    {
        Task CreateAsync(Category category);
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(string id);

        // Byter namn på kategorin OCH på alla poddar som använder den, i samma transaktion
        Task RenameAsync(string id, string oldName, string newName);

        Task DeleteAsync(string id);
    }
}