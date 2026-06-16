using System.Net.Http.Json;
using SoopWorkshop.Shared.DTOs.Tasks;

namespace SoopWorkshop.Frontend.Services.HttpClients
{
    // Kapselt alle API-Class für Aufgaben/Kategorien der Teilnehmer Sicht
    // Gibt nur Sichtbare Kategorien/Aufgaben zurück
    public class TaskApiClient
    {
        private readonly HttpClient _httpClient;

        public TaskApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        // Gibt alle sichtbaren Kategorien mit ihren sichtbaren Aufgaben zurück
        public async Task<List<TaskCategoryDto>> GetVisibleCategoriesAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<TaskCategoryDto>>("api/categories");
            return result ?? [];
        }
        
        // Gibt alle Details einer Aufgabe zurück
        public async Task<TaskItemDto?> GetTaskByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<TaskItemDto>($"api/tasks/{id}");
        }
    }
}