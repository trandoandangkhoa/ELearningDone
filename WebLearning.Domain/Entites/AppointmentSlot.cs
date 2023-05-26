using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebLearning.Domain.Entites
{
    public class AppointmentSlot
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        [JsonIgnore()]
        public Room Room { get; set; }

        [JsonPropertyName("text")]
        public string PatientName { set; get; }

        [JsonPropertyName("patient")]
        public string PatientId { set; get; }

        public string Description { get; set; }

        public string Note { get; set; }

        public string Email { get; set; }

        public string Status { get; set; } = "free";

        [NotMapped]
        public int Resource { get { return Room.Id; } }

        [NotMapped]
        public string DoctorName { get { return Room.Name; } }

        public Guid CodeId { get; set; }
    }
}
