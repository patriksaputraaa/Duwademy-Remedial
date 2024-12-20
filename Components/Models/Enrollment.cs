using System.Text.Json.Serialization;

namespace Duwademy.Components.Models
{
    public class Enrollment
    {
        [JsonPropertyName("enrollmentId")]
        public int Id { get; set; }
        [JsonPropertyName("instructorId")]
        public int InstructorId { get; set; }
        [JsonPropertyName("courseId")]
        public int CourseId { get; set; }
        [JsonPropertyName("enrolledAt")]
        public string EnrolledAt { get; set; }
        public Course Course { get; set; }
        public List<Instructor> Instructors { get; set; } = new();

    }

}
