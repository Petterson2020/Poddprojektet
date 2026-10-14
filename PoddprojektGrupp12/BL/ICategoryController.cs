using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace BL
{
    public interface ICategoryController
    {
        Task CreateCategoryAsync(string name);
        Task<List<Category>> GetAllCategoriesAsync();
        Task DeleteCategoryAsync(string categoryName);
        Task UpdateCategoryAsync(string oldName, string newName); // Denna behövs för Ändra-knappen!
    }
}