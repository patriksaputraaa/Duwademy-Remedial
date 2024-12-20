using System.Text.Json.Serialization;

namespace Duwademy.Components.Models
{
    public class Instructor
    {
        [JsonPropertyName("instructorId")]
        public int Id { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }
    }
}
