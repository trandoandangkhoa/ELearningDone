namespace WebLearning.Contract.Dtos.BookingCalender
{
    public class AppointmentSlotRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Resource { get; set; }

        public string Scale { get; set; }
    }
}
