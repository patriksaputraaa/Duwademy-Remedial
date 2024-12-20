using Duwademy.Components.Models;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Duwademy.Components.Services
{
    public class CategoryService
    {
        private readonly HttpClient httpClient;
        private readonly IJSRuntime jsRuntime;

        public CategoryService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            this.httpClient = httpClient;
            this.jsRuntime = jsRuntime;
        }

        private async Task AddAuthorizationHeaderAsync()
        {
            var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
            Console.WriteLine("Token : " + token);
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            await AddAuthorizationHeaderAsync(); // Add token before making the request

            var response = await httpClient.GetAsync("https://actbackendseervices.azurewebsites.net/api/categories");
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error fetching categories: {errorContent}");
                return new List<Category>();
            }

            return await response.Content.ReadFromJsonAsync<List<Category>>();
        }

        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            await AddAuthorizationHeaderAsync(); // Add token before making the request

            var response = await httpClient.DeleteAsync($"https://actbackendseervices.azurewebsites.net/api/categories/{categoryId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategoryAsync(int categoryId, Category updatedCategory)
        {
            await AddAuthorizationHeaderAsync(); // Add token before making the request

            var response = await httpClient.PutAsJsonAsync($"https://actbackendseervices.azurewebsites.net/api/categories/{categoryId}", updatedCategory);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating category {categoryId}: {errorContent}");
                return false;
            }

            Console.WriteLine($"Category {categoryId} updated successfully.");
            return true;
        }

        // Don't forget to implement GetCategoryByIdAsync in CategoryService:
        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            await AddAuthorizationHeaderAsync(); // Add token before making the request

            var response = await httpClient.GetAsync($"https://actbackendseervices.azurewebsites.net/api/categories/{categoryId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Category>();
            }
            else
            {
                Console.WriteLine($"Error fetching category with ID {categoryId}: {await response.Content.ReadAsStringAsync()}");
                return null;
            }
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            await AddAuthorizationHeaderAsync(); // Add token before making the request

            var response = await httpClient.PostAsJsonAsync("https://actbackendseervices.azurewebsites.net/api/categories", category);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error adding category: {errorContent}");
                return false;
            }

            Console.WriteLine("Category added successfully.");
            return true;
        }

    }
}
