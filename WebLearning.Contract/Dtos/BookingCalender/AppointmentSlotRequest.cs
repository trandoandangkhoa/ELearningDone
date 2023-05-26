namespace WebLearning.Contract.Dtos.BookingCalender
{
    public class AppointmentSlotRequest
    {
        public string Name { get; set; }
        public string Patient { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int DoctorId { get; set; }

    }
}
