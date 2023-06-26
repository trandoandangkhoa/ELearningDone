using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Contract.Dtos.BookingCalender.HistoryAddSlot;

namespace WebLearning.AppUser.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;

        private readonly INotyfService _notyf;
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        private readonly IRoomService _roomService;
        public BookingController(ILogger<BookingController> logger, INotyfService notyf, IBookingService bookingService, IConfiguration configuration, IAccountService accountService, IRoomService roomService)

        {
            _logger = logger;
            _notyf = notyf;
            _bookingService = bookingService;
            _configuration = configuration;
            _accountService = accountService;
            _roomService = roomService;
        }
        [HttpGet]
        [Route("dat-lich.html")]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var a = await _accountService.GetFullName(User.Identity.Name);

            if (a.AuthorizeRole.ToString() != "ManagerRole" && a.AuthorizeRole.ToString() != "AdminRole"
                && a.AuthorizeRole.ToString() != "TeacherRole" && a.AuthorizeRole.ToString() != "StaffRole") { return Redirect("/khong-tim-thay-trang.html"); }
            return View();
        }
        [HttpGet]
        [Route("cap-nhat/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var rs = await _bookingService.GetAppointmentSlotDto(id);
            var room = await _roomService.GetAllRooms();
            ViewData["DanhMuc"] = new SelectList(room, "Id", "Name");

            if (rs != null)
            {
                AppointmentSlotUpdate update = new()
                {
                    Start = rs.Start,
                    End = rs.End,
                    Status = rs.Status,
                    Description = rs.Description,
                    Name = rs.PatientName,
                    Note = rs.Note,
                    Email = rs.Email,
                    Resource = rs.Resource,
                };
                return View(update);
            }
            return View();
        }
        [HttpPost]
        [Route("cap-nhat/{id}")]
        public async Task<IActionResult> Update(int id, AppointmentSlotUpdate appointmentSlotUpdate)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            if (appointmentSlotUpdate.Resource == 0 || appointmentSlotUpdate.Note == null || appointmentSlotUpdate.Description == null)
            {
                _notyf.Error("Vui lòng điền đủ thông tin");
                return Redirect("dat-lich.html/#booknow");
            }
            int compareFromDate = DateTime.Compare(appointmentSlotUpdate.Start, DateTime.Now);
            int compareDateTo = DateTime.Compare(appointmentSlotUpdate.End, DateTime.Now);
            int compareBetweenFromAndDateTo = DateTime.Compare(appointmentSlotUpdate.End, appointmentSlotUpdate.Start);

            if (compareFromDate < 0 || compareDateTo < 0 || compareBetweenFromAndDateTo <= 0)
            {
                _notyf.Error("Thời gian không hợp lệ");
                return Redirect("dat-lich.html/#booknow");
            }
            if (appointmentSlotUpdate.Start.Hour < 8)
            {

                _notyf.Warning("Chưa đến thời gian làm việc");
                return Redirect("dat-lich.html/#booknow");
            }
            if (appointmentSlotUpdate.End.Hour >= 17 && appointmentSlotUpdate.End.Minute > 30 || appointmentSlotUpdate.End.Hour >= 18)
            {

                _notyf.Warning("Hết thời gian làm việc");
                return Redirect("dat-lich.html/#booknow");
            }
            if (appointmentSlotUpdate != null)
            {
                var rs = await _bookingService.PutAppointment(id, appointmentSlotUpdate, User.Identity.Name);
                _notyf.Success(rs.message);

                return Redirect("/dat-lich.html");
            }
            return View(appointmentSlotUpdate);
        }
        [Route("admin.html")]

        public async Task<IActionResult> Admin()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var a = await _accountService.GetFullName(User.Identity.Name);
            if (a.AuthorizeRole.ToString() != "AdminRole") { return Redirect("/khong-tim-thay-trang.html"); }
            return View();
        }
        [Route("manager.html")]

        public async Task<IActionResult> Manager()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var a = await _accountService.GetFullName(User.Identity.Name);
            if (a.AuthorizeRole.ToString() != "ManagerRole" && a.AuthorizeRole.ToString() != "AdminRole"
                && a.AuthorizeRole.ToString() != "TeacherRole") { return Redirect("/khong-tim-thay-trang.html"); }
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
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return null;
            }
            return _configuration.GetValue<string>("BaseAddress");
        }
        public string Name()
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return null;
            }
            if (User.Identity.Name == null) { return "null"; }
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
                if (createAppointmentSlotAdvance.Description != null)
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

            foreach (var item in a)
            {
                if (item.message == "Success")
                {
                    _notyf.Success("Đặt lịch thành công");

                }
                else
                {
                    _notyf.Error(item.message, 600);

                }
            }
            return Redirect("dat-lich.html/#booknow");


        }
        [Route("phan-hoi-lich/{fromId}/{toId}/trang-thai={Status}/accepted")]
        public async Task<IActionResult> Confirm(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _bookingService.ConfirmedBookingAccepted(updateHistoryAddSlotDto, fromId, toId, User.Identity.Name);
            if (result.message == "NotFound")
            {
                _notyf.Error("Lịch đặt này đã được xác nhận");
                return View("Views/Confirm/Error.cshtml");
            }
            _notyf.Success("Gửi mail xác nhận thành công!");
            return View("Views/Confirm/Success.cshtml");
        }
        [Route("phan-hoi-lich/{fromId}/{toId}/trang-thai={Status}/rejected")]
        public async Task<IActionResult> Reject(UpdateHistoryAddSlotDto updateHistoryAddSlotDto, Guid fromId, Guid toId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var result = await _bookingService.ConfirmedBookingRejected(updateHistoryAddSlotDto, fromId, toId, User.Identity.Name);
            if (result.message == "NotFound")
            {
                _notyf.Error("Lịch đặt này đã được xác nhận");
                return View("Views/Confirm/Error.cshtml");
            }
            _notyf.Success("Gửi mail xác nhận thành công!");
            return View("Views/Confirm/Success.cshtml");
        }
        [Route("phan-hoi-doi-lich/{fromId}/{toId}/accepted")]
        public async Task<IActionResult> MoveCalenderAccepted(Guid fromId, Guid toId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var a = await _bookingService.ReplyMoveBookingAccepted(fromId, toId);
            if (a.message == "NotFound")
            {
                _notyf.Error("Lịch đặt này đã được xác nhận");
                return View("Views/Confirm/Error.cshtml");
            }
            _notyf.Success("Gửi mail xác nhận thành công!");
            return View("Views/Confirm/Success.cshtml");
        }
        [Route("phan-hoi-doi-lich/{fromId}/{toId}/rejected")]
        public async Task<IActionResult> MoveCalenderRejected(Guid fromId, Guid toId)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var a = await _bookingService.ReplyMoveBookingRejected(fromId, toId);
            if (a.message == "NotFound")
            {
                _notyf.Error("Lịch đặt này đã được xác nhận");
                return View("Views/Confirm/Error.cshtml");
            }
            _notyf.Success("Gửi mail xác nhận thành công!");
            return View("Views/Confirm/Success.cshtml");
        }


        [Route("lich-theo-ngay-va-tuan/{start}/{end}/{doctor}")]
        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointment(DateTime start, DateTime end, int doctor)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return null;
            }
            var a = await _bookingService.GetAppointmentSlots(start, end, doctor);

            return a;
        }
        [Route("lich-theo-thang/{start}/{end}")]
        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointmentMonth(DateTime start, DateTime end, int doctor)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return null;
            }
            var a = await _bookingService.GetAppointmentSlots(start, end, doctor);

            return a;
        }
        [Route("lich-theo-ngay-va-tuan-manager/{start}/{end}/{doctor}")]
        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointmentManager(DateTime start, DateTime end, int doctor)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return null;
            }
            var a = await _bookingService.GetAppointmentSlotsManager(start, end, doctor);

            return a;
        }
        [Route("lich-theo-thang-manager/{start}/{end}")]
        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointmentMonthManager(DateTime start, DateTime end, int doctor)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return null;
            }
            var a = await _bookingService.GetAppointmentSlotsManager(start, end, doctor);

            return a;
        }
        [Route("lich-theo-thang-admin/{start}/{end}")]
        public async Task<IEnumerable<AppointmentSlotDto>> GetAppointmentMonthAdmin(DateTime start, DateTime end, int doctor)

        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return null;
            }
            var a = await _bookingService.GetAppointmentSlotsAdmin(start, end, doctor);

            return a;
        }
        [Route("xoa-lich/{CodeId}")]
        public async Task<IActionResult> Delete(Guid codeId)
        {
            var rs = await _bookingService.DeleteAppointmentBooked(codeId);

            if (rs.message == "Success")
            {
                _notyf.Success("Xóa lịch thành công!");

                return Redirect("/dat-lich.html");
            }
            _notyf.Error(rs.message);

            return Redirect("./");
        }

        public async Task<IActionResult> Export(DateTime fromDate, DateTime toDate, bool confirmed, string email, int room)
        {
            var token = HttpContext.Session.GetString("Token");

            if (token == null)
            {
                return Redirect("/dang-nhap.html");
            }
            var export = await _bookingService.HistoryAddSlotExports(fromDate, toDate, confirmed, email, room);

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var sheet = package.Workbook.Worksheets.Add("Lịch sử đặt phòng");
                sheet.Cells[1, 1].Value = "Id";
                sheet.Cells[1, 2].Value = "Tên phòng";
                sheet.Cells[1, 3].Value = "Email người dùng";
                sheet.Cells[1, 4].Value = "Tiêu đề";
                sheet.Cells[1, 5].Value = "Nội dung";
                sheet.Cells[1, 6].Value = "Ghi chú";
                sheet.Cells[1, 7].Value = "Bắt đầu";
                sheet.Cells[1, 8].Value = "Kết thúc";
                sheet.Cells[1, 9].Value = "Trạng thái";
                sheet.Cells[1, 10].Value = "Hình thức";
                sheet.Cells[1, 11].Value = "Gửi email";
                sheet.Cells[1, 12].Value = "Người sửa gần nhất";

                int rowInd = 2;

                foreach (var lo in export)
                {
                    sheet.Cells[rowInd, 1].Value = lo.Id;
                    sheet.Cells[rowInd, 2].Value = lo.Room;
                    sheet.Cells[rowInd, 3].Value = lo.Email;
                    sheet.Cells[rowInd, 4].Value = lo.Title;
                    sheet.Cells[rowInd, 5].Value = lo.Description;
                    sheet.Cells[rowInd, 6].Value = lo.Note;
                    sheet.Cells[rowInd, 7].Value = lo.Start.ToString("dd/MM/yyyy HH:mm");
                    sheet.Cells[rowInd, 8].Value = lo.End.ToString("dd/MM/yyyy HH:mm");
                    if (lo.Status == "confirmed")
                    {
                        sheet.Cells[rowInd, 9].Value = "Đã xác nhận";

                    }
                    else if (lo.Status == "waiting")
                    {
                        sheet.Cells[rowInd, 9].Value = "Chờ xác nhận";

                    }
                    if (lo.TypedSubmit == "Create")
                    {
                        sheet.Cells[rowInd, 10].Value = "Tạo mới";

                    }
                    else if (lo.TypedSubmit == "Edit")
                    {
                        sheet.Cells[rowInd, 10].Value = "Chỉnh sửa";

                    }
                    if (lo.SendMail == true)
                    {
                        sheet.Cells[rowInd, 11].Value = "Thành công";

                    }
                    else
                    {
                        sheet.Cells[rowInd, 11].Value = "Không thành công";

                    }
                    sheet.Cells[rowInd, 12].Value = lo.Editor;

                    rowInd++;
                }
                package.Save();
            }

            stream.Position = 0;

            var fileName = $"LichSuDatPhong_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

        }
    }
}
