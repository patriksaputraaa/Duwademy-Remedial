namespace Duwademy.Components.Services
{
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Duwademy.Components.Models;

    public class CourseService
    {
        private readonly HttpClient httpClient;

        public CourseService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            var response = await httpClient.GetAsync("https://actbackendseervices.azurewebsites.net/api/Courses");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error fetching courses: {errorContent}");
                return new List<Course>();
            }

            var coursesList = await response.Content.ReadFromJsonAsync<List<Course>>();
            return coursesList;
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            var response = await httpClient.GetAsync($"https://actbackendseervices.azurewebsites.net/api/Courses/{courseId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Course>();
            }
            else
            {
                Console.WriteLine($"Error fetching course with ID {courseId}: {await response.Content.ReadAsStringAsync()}");
                return null; // Return null if not found or on error
            }
        }

        public async Task<bool> UpdateCourseAsync(int courseId, Course updatedCourse)
        {
            // Check for ID mismatch
            if (courseId != updatedCourse.CourseId)
            {
                Console.WriteLine("Course ID mismatch.");
                return false;
            }

            var response = await httpClient.PutAsJsonAsync($"https://actbackendseervices.azurewebsites.net/api/Courses/{courseId}", updatedCourse);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error updating course {courseId}: {errorContent}");
                return false;
            }

            Console.WriteLine($"Course {courseId} updated successfully.");
            return true;
        }


        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var response = await httpClient.DeleteAsync($"https://actbackendseervices.azurewebsites.net/api/Courses/{courseId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error deleting course {courseId}: {errorContent}");
                return false; // Return false if deletion failed
            }

            Console.WriteLine($"Course {courseId} deleted successfully.");
            return true; // Return true if deletion was successful
        }


        public async Task<bool> AddCourseAsync(Course course)
        {
            var response = await httpClient.PostAsJsonAsync("https://actbackendseervices.azurewebsites.net/api/Courses", course);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error adding course: {errorContent}");
                return false;
            }

            Console.WriteLine("Course added successfully.");
            return true;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await httpClient.GetAsync("https://actbackendseervices.azurewebsites.net/api/Categories");
            //response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error fetching courses: {errorContent}");
                return new List<Category>();
            }

            var categoryList = await response.Content.ReadFromJsonAsync<List<Category>>();
            return categoryList;
        }

        public bool DoesImageExist(string imageName)
        {
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageName);
            return File.Exists(imagePath);
        }

        public async Task<List<Course>> SearchCoursesByNameAsync(string courseName)
        {
            try
            {
                var response = await httpClient.GetAsync($"https://actbackendseervices.azurewebsites.net/api/Courses/search/{Uri.EscapeDataString(courseName)}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error searching courses by name: {errorContent}");
                    return new List<Course>();
                }

                var courses = await response.Content.ReadFromJsonAsync<List<Course>>();
                return courses ?? new List<Course>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while searching courses: {ex.Message}");
                return new List<Course>();
            }
        }


    }
}
