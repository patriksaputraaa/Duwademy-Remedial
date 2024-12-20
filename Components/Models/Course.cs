using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Duwademy.Components.Models
{
    public class Course
    {
        [JsonPropertyName("courseId")]
        public int CourseId { get; set; }

        [JsonPropertyName("name")]
        [Required(ErrorMessage = "Course name is required.")]
        public string Name { get; set; }

        [JsonPropertyName("imageName")]
        public string? ImageName { get; set; }

        [JsonPropertyName("duration")]
        [Required(ErrorMessage = "Duration is required.")]
        public int? Duration { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("categoryId")]
        [Required(ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        [JsonPropertyName("category")]
        public Category? Category { get; set; } // Reference to the Category class

    }
}
