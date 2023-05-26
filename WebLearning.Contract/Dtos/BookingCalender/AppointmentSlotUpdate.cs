namespace WebLearning.Contract.Dtos.BookingCalender
{
    public class AppointmentSlotUpdate
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Resource { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public int CustomAdd { get; set; }

        public string Email { get; set; }
        public string Status { get; set; }

    }
}
