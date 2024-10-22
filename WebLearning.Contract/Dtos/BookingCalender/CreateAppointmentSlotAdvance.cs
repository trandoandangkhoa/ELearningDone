﻿namespace WebLearning.Contract.Dtos.BookingCalender
{
    public class CreateAppointmentSlotAdvance
    {
        public string Email { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Title { get; set; }
        public string Note { get; set; }

        public string Description { get; set; }

        public int Room { get; set; }

        public int TypedSubmit { get; set; }

    }
}
