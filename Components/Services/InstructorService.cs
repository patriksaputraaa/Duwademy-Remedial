using Duwademy.Components.Models;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Duwademy.Components.Services
{
    public class InstructorService
    {
        private readonly HttpClient httpClient;

        public InstructorService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Instructor>> GetInstructorsAsync()
        {
            var response = await httpClient.GetAsync("https://actbackendseervices.azurewebsites.net/api/instructors");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error fetching instructor: {errorContent}");
                return new List<Instructor>();
            }

            var instructorList = await response.Content.ReadFromJsonAsync<List<Instructor>>();
            return instructorList;
        }
    }
}
