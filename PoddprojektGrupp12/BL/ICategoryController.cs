using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace BL
{
    public interface ICategoryController
    {
        Task CreateCategoryAsync(string name);
        Task<List<Category>> GetAllCategoriesAsync();
        Task UpdateCategoryAsync(string id, string name);
        Task DeleteCategoryAsync(string id);
    }
}
