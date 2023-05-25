using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Numerics;
using WebLearning.ApiIntegration.Services;
using WebLearning.Application.Helper;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Contract.Dtos.BookingCalender.HistoryAddSlot;
using WebLearning.Domain.Entites;

namespace WebLearning.AppUser.Controllers
{
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;

        private readonly INotyfService _notyf;
        private readonly IConfiguration _configuration;
        public BookingController(ILogger<BookingController> logger, INotyfService notyf, IBookingService bookingService, IConfiguration configuration)

        {
            _logger = logger;
            _notyf = notyf;
            _bookingService = bookingService;
            _configuration = configuration;
        }
        [HttpGet]
        [Route("dat-lich.html")]
        public IActionResult Index()
        {
            CreateAppointmentSlotAdvance createAppointmentSlotAdvance = new();
            return View(createAppointmentSlotAdvance);
        }
        [Route("admin.html")]

        public IActionResult Admin()
        {
            return View();
        }
        [Route("manager.html")]

        public IActionResult Manager()
        {
            return View();
        }

        [Route("dat-thanh-cong.html", Name = "BookSuccessed")]
        public async Task<IActionResult> BookSuccessed()
        {
            var khachhang = await _bookingService.BookSuccessed(User.Identity.Name);
            return View(khachhang);
        }
        public string Address()
        {
            return _configuration.GetValue<string>("BaseAddress");
        }
        public string Name()
        {
            return User.Identity.Name.ToString();
        }
        [HttpPost]
        [Route("dat-lich.html")]
        public async Task<ActionResult> Add(CreateAppointmentSlotAdvance createAppointmentSlotAdvance)
        {
            if (createAppointmentSlotAdvance.Room == 0 || createAppointmentSlotAdvance.Note == null || createAppointmentSlotAdvance.Description == null)
            {
                if (createAppointmentSlotAdvance.Title != null)
                {
                    TempData["Title"] = createAppointmentSlotAdvance.Title;

                }
                if (createAppointmentSlotAdvance.Note != null)
                {
                    TempData["Note"] = createAppointmentSlotAdvance.Note;

                }
                if(createAppointmentSlotAdvance.Description != null)
                {
                    TempData["Description"] = createAppointmentSlotAdvance.Description;

                }

                TempData["FromDate"] = createAppointmentSlotAdvance.Start;
                TempData["ToDate"] = createAppointmentSlotAdvance.End;
                TempData["CustomAdd"] = createAppointmentSlotAdvance.TypedSubmit;

                _notyf.Error("Vui lòng điền đủ thông tin");
                return Redirect("dat-lich.html/#booknow");
            }
            int compareFromDate = DateTime.Compare(createAppointmentSlotAdvance.Start, DateTime.Now);
            int compareDateTo = DateTime.Compare(createAppointmentSlotAdvance.End, DateTime.Now);
            int compareBetweenFromAndDateTo = DateTime.Compare(createAppointmentSlotAdvance.End, createAppointmentSlotAdvance.Start);

            if (compareFromDate < 0 || compareDateTo < 0 || compareBetweenFromAndDateTo <= 0)
            {

                TempData["Title"] = createAppointmentSlotAdvance.Title;
                TempData["Note"] = createAppointmentSlotAdvance.Note;
                TempData["Description"] = createAppointmentSlotAdvance.Description;

                TempData["FromDate"] = createAppointmentSlotAdvance.Start;
                TempData["ToDate"] = createAppointmentSlotAdvance.End;
                TempData["CustomAdd"] = createAppointmentSlotAdvance.TypedSubmit;


                _notyf.Error("Thời gian không hợp lệ");
                return Redirect("dat-lich.html/#booknow");
            }
            if (createAppointmentSlotAdvance.Start.Hour < 8)
            {
                TempData["Note"] = createAppointmentSlotAdvance.Note;
                TempData["Description"] = createAppointmentSlotAdvance.Description;
                TempData["Title"] = createAppointmentSlotAdvance.Title;

                TempData["FromDate"] = createAppointmentSlotAdvance.Start;
                TempData["ToDate"] = createAppointmentSlotAdvance.End;
                TempData["CustomAdd"] = createAppointmentSlotAdvance.TypedSubmit;

                _notyf.Warning("Chưa đến thời gian làm việc");
                return Redirect("dat-lich.html/#booknow");
            }
            if (createAppointmentSlotAdvance.End.Hour >= 17 && createAppointmentSlotAdvance.End.Minute > 30 || createAppointmentSlotAdvance.End.Hour >= 18)
            {
                TempData["Note"] = createAppointmentSlotAdvance.Note;
                TempData["Description"] = createAppointmentSlotAdvance.Description;
                TempData["Title"] = createAppointmentSlotAdvance.Title;

                TempData["FromDate"] = createAppointmentSlotAdvance.Start;
                TempData["ToDate"] = createAppointmentSlotAdvance.End;
                TempData["CustomAdd"] = createAppointmentSlotAdvance.TypedSubmit;

                _notyf.Warning("Hết thời gian làm việc");
                return Redirect("dat-lich.html/#booknow");
            }
            var a = await _bookingService.CreateAppointmentSlotAdvance(createAppointmentSlotAdvance);

            foreach(var item in a)
            {
                if(item.message == "Success")
                {
                    _notyf.Success("Đặt lịch thành công");

                }
                else
                {
                    _notyf.Error(item.message,600);

                }
            }
            return Redirect("dat-lich.html/#booknow");


        }


        [Route("phan-hoi-lich/{fromId}/{toId}/trang-thai={Status}/accepted")]
        public async Task<IActionResult> Confirm(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId)
        {
            if (User.Identity.Name == null) return Redirect("/dang-nhap.html");

            var result = await _bookingService.ConfirmedBookingAccepted(updateHistoryAddSlotDto,fromId,toId,User.Identity.Name);

            return View("Views/Confirm/Success.cshtml");
        }
        [Route("phan-hoi-lich/{fromId}/{toId}/trang-thai={Status}/rejected")]
        public async Task<IActionResult> Reject(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId)
        {
            if (User.Identity.Name == null) return Redirect("/dang-nhap.html");

            var result = await _bookingService.ConfirmedBookingRejected(updateHistoryAddSlotDto, fromId, toId, User.Identity.Name);

            return View("Views/Confirm/Success.cshtml");
        }
        [Route("phan-hoi-doi-lich/{fromId}/{toId}/accepted")]
        public async Task<IActionResult> MoveCalenderAccepted(Guid fromId, Guid toId)
        {

            var a = await _bookingService.ReplyMoveBookingAccepted(fromId, toId);

            return View("Views/Confirm/Success.cshtml");
        }
        [Route("phan-hoi-doi-lich/{fromId}/{toId}/rejected")]
        public async Task<IActionResult> MoveCalenderRejected(Guid fromId, Guid toId)
        {

            var a = await _bookingService.ReplyMoveBookingRejected(fromId, toId);

            return View("Views/Confirm/Success.cshtml");
        }


        [Route("lich-theo-ngay-va-tuan/{start}/{end}/{doctor}")]
        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointment(DateTime start, DateTime end,int doctor)
        {

            var a = await _bookingService.GetAppointmentSlots(start,end,doctor);

            return a;
        }
        [Route("lich-theo-thang/{start}/{end}")]
        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointmentMonth(DateTime start, DateTime end, int doctor)
        {
            var a = await _bookingService.GetAppointmentSlots(start, end, doctor);

            return a;
        }
        [Route("xoa-lich/{CodeId}")]
        public async Task<IActionResult> Delete(Guid codeId)
        {
            var rs = await _bookingService.DeleteAppointmentBooked(codeId);

            if(rs.message == "Success")
            {
                _notyf.Success("Xóa lịch thành công!");

                return Redirect("/dat-lich.html");
            }
            _notyf.Error(rs.message);

            return Redirect("./");
        }
        //[Route("doi-lich/{fromId}/{toId}/rejected")]
        //public async Task<IActionResult> MoveCalenderRejected(Guid fromId, Guid toId)
        //{
        //    var history = await _context.HistoryAddSlots.SingleOrDefaultAsync(x => x.CodeId.Equals(toId));

        //    if (history == null)
        //    {
        //        return NotFound();
        //    }
        //    var room = await _context.Rooms.SingleOrDefaultAsync(x => x.Id == history.RoomId);
        //    var infouser = await _context.Accounts.SingleOrDefaultAsync(x => x.Email.Contains(history.Email));



        //    var emailAdmin = _configuration.GetValue<string>("EmailConfiguration:To");

        //    var baseAddress = _configuration.GetValue<string>("BaseAddress");


        //    var body = "Nội dung cuộc họp: " + history.Description + "<br/>" +
        //                "Ghi chú: " + history.Note + "<br/>" +
        //                "Họ tên người đặt: " + infouser.Name + "<br/>" +
        //                "Email: " + infouser.Email + "<br/>" +
        //                "Bộ phận: " + infouser.Department + "<br/>" +
        //                "Phòng: " + room.Name + "<br/>" +
        //                "Thời gian bắt đầu: " + history.Start + "<br/>" +
        //                "Thời gian kết thúc: " + history.End + "<br/>" +
        //                "Trang thái: " + "<span style='background-color:#d9534f;display: inline;padding: .3em .7em .4em;font-size: 75%;font-weight: 700;line-height: 1;color: #fff;text-align: center;white-space: nowrap;vertical-align: baseline;border-radius: .25em;'>" +
        //                            "Từ chối" +
        //                            "</span>" + "<br/><br/>" +
        //                "<div>" +
        //                    "<a style='padding: 6px 12px;background-color: #5bc0de;border-radius:30px;text-decoration: none;color:#fff;' " +
        //                    "href = " + baseAddress + fromId + "/" + toId + "&trang-thai=0" + " >Duyệt</a>" + "<br/><br/>" +
        //                 "</div>" + "</div></div>";

        //    var message = new Message(new string[] { $"{emailAdmin}" }, "XÁC NHẬN DỜI LỊCH", $"{body}");

        //    _emailSender.ReplyEmail(message, infouser.Email, infouser.Name, emailAdmin);

        //    return View("Views/Confirm/Success.cshtml");
        //}
    }
}
