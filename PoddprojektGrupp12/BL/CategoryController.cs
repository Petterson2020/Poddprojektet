using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using Models;

namespace BL
{
    public class CategoryController : ICategoryController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPodcastRepository _podcastRepository;

        public CategoryController() : this(new CategoryRepository(), new PodcastRepository()) { }

        public CategoryController(ICategoryRepository categoryRepository, IPodcastRepository podcastRepository)
        {
            _categoryRepository = categoryRepository;
            _podcastRepository = podcastRepository;
        }

        public async Task CreateCategoryAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Kategorinamnet får inte vara tomt.");
            }

            var category = new Category
            {
                Id = Guid.NewGuid().ToString(),
                Name = name.Trim()
            };

            await _categoryRepository.CreateAsync(category);
        }

        public Task<List<Category>> GetAllCategoriesAsync() => _categoryRepository.GetAllAsync();

        public async Task UpdateCategoryAsync(string id, string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Det nya kategorinamnet får inte vara tomt.");
            }

            Category category = await _categoryRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Kunde inte hitta kategorin som skulle ändras.");

            await _categoryRepository.RenameAsync(id, category.Name, newName.Trim());
        }

        public async Task DeleteCategoryAsync(string id)
        {
            Category category = await _categoryRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Kunde inte hitta kategorin som skulle tas bort.");

            long count = await _podcastRepository.CountByCategoryAsync(category.Name);
            if (count > 0)
            {
                throw new CategoryInUseException(
                    $"Kategorin '{category.Name}' används av {count} podd(ar). Ändra kategori på dem eller ta bort dem först.");
            }

            await _categoryRepository.DeleteAsync(id);
        }
    }
}