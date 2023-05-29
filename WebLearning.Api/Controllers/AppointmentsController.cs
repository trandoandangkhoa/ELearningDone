using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLearning.Application.BookingCalender.Services;
using WebLearning.Application.Email;
using WebLearning.Application.Helper;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Contract.Dtos.BookingCalender.HistoryAddSlot;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ApiBase
    {
        private readonly WebLearningContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly IAppointmentService _appointmentService;
        public AppointmentsController(WebLearningContext context, IEmailSender emailSender, IConfiguration configuration, IAppointmentService appointmentService)
        {
            _context = context;
            _emailSender = emailSender;
            _configuration = configuration;
            _appointmentService = appointmentService;

        }

        // GET: api/Appointments
        /// <summary>
        /// Danh sách tất cả thời gian có thể đặt của mỗi phòng
        /// </summary>
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StaffRole, AuthorizeRole.ManagerRole, AuthorizeRole.TeacherRole)]
        [HttpGet]
        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointments([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] int? doctor)
        {
            return await _appointmentService.GetAppointments(start, end, doctor);

        }
        // GET: api/Appointments
        /// <summary>
        /// Danh sách tất cả thời gian có thể đặt của mỗi phòng (manager)
        /// </summary>
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ManagerRole)]
        [HttpGet("manager")]
        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointmentsManager([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] int? doctor)
        {
            return await _appointmentService.GetAppointments(start, end, doctor);

        }
        // GET: api/Appointments
        /// <summary>
        /// Danh sách tất cả thời gian có thể đặt của mỗi phòng (admin)
        /// </summary>
        [SecurityRole(AuthorizeRole.AdminRole)]
        [HttpGet("admin")]
        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointmentsAdmin([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] int? doctor)
        {
            return await _appointmentService.GetAppointments(start, end, doctor);

        }
        /// <summary>
        /// Danh sách thời gian trống của mỗi phòng
        /// </summary>
        [HttpGet("free")]
        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointments([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] string patient)
        {
            return await _appointmentService.AppointmentsFree(start, end, patient);
        }
        /// <summary>
        /// Lấy thông tin chi tiết theo id vị trí
        /// </summary>
        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentSlotDto>> GetAppointmentSlot(int id)
        {
            try
            {
                var a = await _appointmentService.GetAppointmentSlot(id);
                if (a == null) return NotFound();
                return Ok(a);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        /// <summary>
        /// Cập nhật trạng thái cho từng mốc thời gian cũng như dời lịch
        /// </summary>
        // PUT: api/Appointments/5
        [HttpPut("{id}/{email}")]
        public async Task<IActionResult> PutAppointmentSlot(int id, AppointmentSlotUpdate update, string email)
        {
            var result = await _appointmentService.PutAppointment(id, update, email);

            return StatusCode(StatusCodes.Status200OK, result);

        }
        /// <summary>
        /// Đặt lịch họp mới
        /// </summary>
        // PUT: api/Appointments/5
        [HttpPut("{id}/request/name")]
        public async Task<IActionResult> PutAppointmentSlotRequest(int id, AppointmentSlotRequest slotRequest, string name)
        {
            var result = await _appointmentService.CreateAppointment(id, slotRequest, name);

            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        /// Tạo thời gian trống cho từng phòng
        /// </summary>
        // POST: api/Appointments
        //[HttpPost]
        //public async Task<ActionResult<AppointmentSlot>> PostAppointmentSlot(AppointmentSlot appointmentSlot)
        //{
        //    _context.Appointments.Add(appointmentSlot);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetAppointmentSlot", new { id = appointmentSlot.Id }, appointmentSlot);
        //}

        /// <summary>
        /// Tạo thời gian trống cho từng phòng
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<AppointmentSlot>> PostAppointmentSlots(AppointmentSlotRange range)
        {
            var rs = await _appointmentService.AdminCreateNewSingleAppointment(range);

            return StatusCode(StatusCodes.Status200OK, rs);
        }
        /// <summary>
        /// Tạo thời gian trống cho từng phòng theo tháng
        /// </summary>        
        [HttpPost("create/months")]
        public async Task<ActionResult<AppointmentSlot>> PostAppointmentFreeSlots(AppointmentSlotRange range)
        {
            var rs = await _appointmentService.CreateSlotInSixMonth(range);

            return StatusCode(StatusCodes.Status200OK, rs);
        }
        /// <summary>
        /// Đặt lịch họp mới theo chế độ nâng cao
        /// </summary>
        [HttpPost("create/advance")]
        public async Task<ActionResult> AddMultiAppointment([FromBody] CreateAppointmentSlotAdvance createAppointmentSlotAdvance)
        {
            try
            {
                var a = await _appointmentService.CreateAppointmentSlotAdvance(createAppointmentSlotAdvance);

                return StatusCode(StatusCodes.Status200OK, a);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);

            }
        }
        /// <summary>
        /// Xóa thời gian trống của từng phòng trong tháng
        /// </summary>
        [HttpPost("clear")]
        public async Task<ActionResult> PostAppointmentClear(ClearRange range)
        {
            var result = await _appointmentService.AdminDeleteMultiAppointment(range);

            return StatusCode(StatusCodes.Status200OK, result);
        }
        /// <summary>
        /// Xóa thời gian trống của từng phòng theo vị trí
        /// </summary>
        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointmentSlot(int id)
        {
            var rs = await _appointmentService.DeleteSingleAppointment(id);
            return StatusCode(StatusCodes.Status200OK, rs);
        }
        /// <summary>
        /// Xóa lịch đặt phòng
        /// </summary>
        // DELETE: api/Appointments/5
        [HttpDelete("booked/{codeId}")]
        public async Task<IActionResult> DeleteAppointmentBooked(Guid codeId)
        {
            var rs = await _appointmentService.DeleteAppointmentBooked(codeId);

            return StatusCode(StatusCodes.Status200OK, rs);
        }
        /// <summary>
        /// Lấy thông tin lịch sử đặt phòng theo từng User
        /// </summary>
        [HttpGet("booked/successed/email")]
        public async Task<IEnumerable<HistoryAddSlot>> BookSuccessed(string email)
        {
            var khachhang = await _context.HistoryAddSlots.OrderByDescending(x => x.Start).ThenBy(x => x.Status).AsNoTracking().Where(x => x.Email.Equals(email) && x.Status != "free").Take(10).ToListAsync();

            return khachhang;

        }
        /// <summary>
        /// Xác nhận trạng thái đặt lịch
        /// </summary>
        [HttpPut("confirmed/accepted/{fromId}/{toId}/{email}")]
        public async Task<ActionResult<Result>> ConfirmedBookingAccepted(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email)
        {
            var a = await _appointmentService.ConfirmBookingAccepted(updateHistoryAddSlotDto, fromId, toId, email);

            return StatusCode(StatusCodes.Status200OK, a);
        }
        [HttpPut("confirmed/rejected/{fromId}/{toId}/{email}")]
        public async Task<ActionResult<Result>> ConfirmedBookingRejected(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId, string email)
        {
            var a = await _appointmentService.ConfirmBookingRejected(updateHistoryAddSlotDto, fromId, toId, email);

            return StatusCode(StatusCodes.Status200OK, a);
        }
        /// <summary>
        /// Chấp nhận dời lịch
        /// </summary>
        [HttpGet("mailreplied/accepted/{fromId}/{toId}")]
        public async Task<IActionResult> MoveCalenderAccepted(Guid fromId, Guid toId)
        {
            var a = await _appointmentService.MailReplyAccepted(fromId, toId);

            return StatusCode(StatusCodes.Status200OK, a);
        }
        /// <summary>
        /// Từ chối dời lịch
        /// </summary>
        [HttpGet("mailreplied/rejected/{fromId}/{toId}")]
        public async Task<IActionResult> MoveCalenderRejected(Guid fromId, Guid toId)
        {
            var a = await _appointmentService.MailReplyRejected(fromId, toId);

            return StatusCode(StatusCodes.Status200OK, a);
        }
        /// <summary>
        /// Xuất excel
        /// </summary>
        [HttpGet("exportExcel")]
        //[SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.ManagerRole)]
        public async Task<IEnumerable<HistoryAddSlotExport>> ExportV2(CancellationToken cancellationToken, DateTime fromDate, DateTime toDate, bool confirmed, string email, int room)
        {
            var data = await _appointmentService.ExportHistoryAllSlotDtos(fromDate, toDate, confirmed, room, email);

            if (data != null)
            {
                return data;
            }
            return default;
        }
    }

}
