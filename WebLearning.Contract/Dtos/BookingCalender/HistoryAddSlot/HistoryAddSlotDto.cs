using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLearning.Contract.Dtos.BookingCalender.Room;

namespace WebLearning.Contract.Dtos.BookingCalender.HistoryAddSlot
{
    public class HistoryAddSlotDto
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public Guid CodeId { get; set; }

        public Guid OldCodeId { get; set; }

        public string Email { get; set; }
        public string Title { get; set; }
        public string Editor { get; set; }
        public string Description { get; set; }

        public string Note { get; set; }
        
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string TypedSubmit { get; set; }

        public string Status { get; set; }

        public RoomDto Room { get; set; }
    }
}
