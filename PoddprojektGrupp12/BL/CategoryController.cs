using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL; // Ger oss tillgång till ICategoryRepository och CategoryRepository
using Models; // Ger oss tillgång till Category

namespace BL
{
    public class CategoryController : ICategoryController
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController()
        {
            // Initiera kopplingen till DAL när controllern skapas
            _categoryRepository = new CategoryRepository();
        }

        public async Task CreateCategoryAsync(string name)
        {
            // 1. Affärslogik & Validering
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Kategorinamnet får inte vara tomt.");
            }

            // 2. Skapa modellen
            Category newCategory = new Category
            {
                Id = Guid.NewGuid().ToString(), // Ger kategorin ett unikt ID för MongoDB
                Name = name
            };

            // 3. Spara i databasen via DAL
            await _categoryRepository.CreateAsync(newCategory);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            // Skicka bara vidare förfrågan till DAL
            return await _categoryRepository.GetAllAsync();
        }

        // UPPDATERAD: Tar nu emot oldName och newName istället för id
        public async Task UpdateCategoryAsync(string oldName, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Det nya kategorinamnet får inte vara tomt.");
            }

            // 1. Hämta alla kategorier för att hitta den vi vill ändra
            var allCategories = await _categoryRepository.GetAllAsync();
            var categoryToUpdate = System.Linq.Enumerable.FirstOrDefault(allCategories, c => c.Name == oldName);

            if (categoryToUpdate != null)
            {
                // 2. Byt ut namnet till det nya
                categoryToUpdate.Name = newName;

                // 3. Skicka den uppdaterade kategorin till databasen
                await _categoryRepository.UpdateAsync(categoryToUpdate);
            }
            else
            {
                throw new Exception("Kunde inte hitta kategorin som skulle ändras.");
            }
        }

        public async Task DeleteCategoryAsync(string categoryName)
        {
            var allCategories = await _categoryRepository.GetAllAsync();
            var categoryToDelete = System.Linq.Enumerable.FirstOrDefault(allCategories, c => c.Name == categoryName);

            if (categoryToDelete != null)
            {
                await _categoryRepository.DeleteAsync(categoryToDelete.Id);
            }
            else
            {
                throw new Exception("Kunde inte hitta kategorin i databasen.");
            }
        }
    }
}