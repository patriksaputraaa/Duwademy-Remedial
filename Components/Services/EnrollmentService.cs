using Duwademy.Components.Models;
using Duwademy.Components.Services;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Duwademy.Components.Services
{
    public class EnrollmentService
    {
        private readonly HttpClient _httpClient;

        public EnrollmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Enrollment>> GetEnrollmentsAsync()
        {
            // Fetch all enrollment data
            var enrollments = await _httpClient.GetFromJsonAsync<List<Enrollment>>("https://actbackendseervices.azurewebsites.net/api/enrollments");
            if (enrollments == null) return new List<Enrollment>();

            var allInstructors = await GetInstructorsAsync();

            foreach (var enrollment in enrollments)
            {
                var course = await GetCourseByIdAsync(enrollment.CourseId);
                enrollment.Course = new Course
                {
                    CourseId = enrollment.CourseId,
                    Name = course.Name,
                    Description = course.Description,
                };

                // Assign instructors by matching the InstructorId
                var instructor = allInstructors.FirstOrDefault(i => i.Id == enrollment.InstructorId);
                if (instructor != null)
                {
                    enrollment.Instructors.Add(instructor);
                }
            }

            // Now, simply return the list of enrollments with the related course and instructor data
            return enrollments;
        }


        public async Task<List<Instructor>> GetInstructorsAsync()
        {
            try
            {
                // Send a GET request to fetch the instructors
                var response = await _httpClient.GetAsync("https://actbackendseervices.azurewebsites.net/api/instructors");

                // Check if the response is successful
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error fetching instructors: {errorContent}");
                    return new List<Instructor>(); // Return an empty list in case of error
                }

                // Deserialize the response content into a list of Instructor objects
                var instructorList = await response.Content.ReadFromJsonAsync<List<Instructor>>();

                // If the response content is null, return an empty list
                return instructorList ?? new List<Instructor>();
            }
            catch (Exception ex)
            {
                // Log any exceptions that might occur during the HTTP request or deserialization
                Console.WriteLine($"Exception fetching instructors: {ex.Message}");
                return new List<Instructor>(); // Return an empty list in case of error
            }
        }


        public async Task<List<Course>> GetCoursesAsync()
        {
            var response = await _httpClient.GetAsync("https://actbackendseervices.azurewebsites.net/api/Courses");

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
            var response = await _httpClient.GetAsync($"https://actbackendseervices.azurewebsites.net/api/Courses/{courseId}");

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
    }
}
