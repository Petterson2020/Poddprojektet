using Models;

namespace DAL
{
    public interface ICategoryRepository
    {
        Task CreateAsync(Category category);
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(string id);

        Task RenameAsync(string id, string oldName, string newName);

        Task DeleteAsync(string id, string name);
    }
}