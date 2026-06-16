using System.Net.Http.Json;
using SoopWorkshop.Shared.DTOs.Evaluation;
using SoopWorkshop.Shared.DTOs.Submissions;
using Microsoft.AspNetCore.Components.Forms;

namespace SoopWorkshop.Frontend.Services.HttpClients
{
    // Kapselt die API-Calls für Submission und Auswertungsergebnisse
    public class SubmissionApiClient
    {
        private readonly HttpClient _httpClient;

        public SubmissionApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Läd ein/mehre .java Dateien für eine Aufgabe hoch und gibt die erstelle Submission zurück
        public async Task<SubmissionDto?> SubmitAsync(Guid taskItemId, IEnumerable<IBrowserFile> files)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(taskItemId.ToString()), "taskItemId");

            foreach (var file in files)
            {
                var stream = file.OpenReadStream(maxAllowedSize: 1024 * 1024); // Max 1MB pro Datei
                content.Add(new StreamContent(stream), "files", file.Name);
            }

            var response = await _httpClient.PostAsync("api/submissions", content);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<SubmissionDto>();
        }
        
        // Ruft das Auswertungsergebnis einer Submission ab
        // null wenn Auswertung noch am laufen ist
        public async Task<EvaluationResultDto?> GetResultAsync(Guid submissionId)
        {
            var response = await _httpClient.GetAsync($"api/submissions/{submissionId}/result");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<EvaluationResultDto>();
        }
    }
}