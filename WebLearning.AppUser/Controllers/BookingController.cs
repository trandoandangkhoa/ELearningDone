using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Contract.Dtos.BookingCalender.HistoryAddSlot;
using WebLearning.Domain.Entites;

namespace WebLearning.AppUser.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;

        private readonly INotyfService _notyf;
        public BookingController(ILogger<BookingController> logger, INotyfService notyf, IBookingService bookingService)

        {
            _logger = logger;
            _notyf = notyf;
            _bookingService = bookingService;
        }
        [Route("dat-lich.html")]
        public IActionResult Index()
        {
            return View();
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
        [Route("/tao-moi.html")]
        public async Task<ActionResult> Add(CreateAppointmentSlotAdvance createAppointmentSlotAdvance)
        {
            createAppointmentSlotAdvance.Email = User.Identity.Name;

            if (createAppointmentSlotAdvance.Room == 0 || createAppointmentSlotAdvance.Note == null || createAppointmentSlotAdvance.Description == null)
            {
                if(createAppointmentSlotAdvance.Note != null)
                {
                    TempData["Note"] = createAppointmentSlotAdvance.Note.ToString();

                }
                if(createAppointmentSlotAdvance.Description != null)
                {
                    TempData["Description"] = createAppointmentSlotAdvance.Description.ToString();

                }

                TempData["FromDate"] = createAppointmentSlotAdvance.Start;
                TempData["ToDate"] = createAppointmentSlotAdvance.End;
                TempData["CustomAdd"] = createAppointmentSlotAdvance.TypedSubmit.ToString();

                _notyf.Error("Vui lòng điền đủ thông tin");
                return Redirect("dat-lich.html/#booknow");
            }
            int compareFromDate = DateTime.Compare(createAppointmentSlotAdvance.Start, DateTime.Now);
            int compareDateTo = DateTime.Compare(createAppointmentSlotAdvance.End, DateTime.Now);
            int compareBetweenFromAndDateTo = DateTime.Compare(createAppointmentSlotAdvance.End, createAppointmentSlotAdvance.Start);

            if (compareFromDate < 0 || compareDateTo < 0 || compareBetweenFromAndDateTo <= 0)
            {
                TempData["Note"] = createAppointmentSlotAdvance.Note.ToString();
                TempData["Description"] = createAppointmentSlotAdvance.Description.ToString();

                TempData["FromDate"] = createAppointmentSlotAdvance.Start;
                TempData["ToDate"] = createAppointmentSlotAdvance.End;
                TempData["CustomAdd"] = createAppointmentSlotAdvance.TypedSubmit.ToString();


                _notyf.Error("Thời gian không hợp lệ");
                return Redirect("dat-lich.html/#booknow");
            }
            if (createAppointmentSlotAdvance.Start.Hour < 8)
            {
                TempData["Note"] = createAppointmentSlotAdvance.Note.ToString();
                TempData["Description"] = createAppointmentSlotAdvance.Description.ToString();

                TempData["FromDate"] = createAppointmentSlotAdvance.Start;
                TempData["ToDate"] = createAppointmentSlotAdvance.End;
                TempData["CustomAdd"] = createAppointmentSlotAdvance.TypedSubmit.ToString();

                _notyf.Warning("Chưa đến thời gian làm việc");
                return Redirect("dat-lich.html/#booknow");
            }
            if (createAppointmentSlotAdvance.End.Hour >= 17 && createAppointmentSlotAdvance.End.Minute > 30 || createAppointmentSlotAdvance.End.Hour >= 18)
            {
                TempData["Note"] = createAppointmentSlotAdvance.Note.ToString();
                TempData["Description"] = createAppointmentSlotAdvance.Description.ToString();

                TempData["FromDate"] = createAppointmentSlotAdvance.Start;
                TempData["ToDate"] = createAppointmentSlotAdvance.End;
                TempData["CustomAdd"] = createAppointmentSlotAdvance.TypedSubmit.ToString();

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

            var a = _bookingService.ReplyBookingAccepted(fromId, toId);

            return View("Views/Confirm/Success.cshtml");
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
