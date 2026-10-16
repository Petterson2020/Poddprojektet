using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace BL
{
    public interface IPodcastController
    {
        // Sparar en ny podd i databasen
        Task AddPodcastAsync(string url, string categoryName);

        // Hämtar alla sparade poddar från databasen
        Task<List<Podcast>> GetAllPodcastsAsync();

        // NY: Hämtar och förhandsgranskar en podd från nätet (för Hämta-knappen)
        Task<Podcast> GetPodcastPreviewAsync(string url);
        Task DeletePodcastAsync(string title); // (Eller DeleteAsync i Repositoryt)
        Task UpdatePodcastAsync(string oldTitle, string newTitle, string newCategory);

    }
}