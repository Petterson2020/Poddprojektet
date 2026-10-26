using Models;

namespace BL
{
    public interface ICategoryController
    {
        Task CreateCategoryAsync(string name);
        Task<List<Category>> GetAllCategoriesAsync();
        Task UpdateCategoryAsync(string id, string newName);
        Task DeleteCategoryAsync(string id);
    }
}