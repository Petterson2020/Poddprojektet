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
                // Detta fel kan du sedan fånga med try/catch i Form1 och visa som en MessageBox
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

        public async Task UpdateCategoryAsync(string id, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Det nya kategorinamnet får inte vara tomt.");
            }

            Category updatedCategory = new Category
            {
                Id = id,
                Name = newName
            };

            await _categoryRepository.UpdateAsync(updatedCategory);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            // OBS! Enligt kraven måste användaren få en bekräftelsefråga när de raderar.
            // Själva MessageBox-frågan ("Är du säker?") bygger du i Form1, 
            // men om de klickar "Ja", anropas den här metoden.

            await _categoryRepository.DeleteAsync(id);
        }
    }
}