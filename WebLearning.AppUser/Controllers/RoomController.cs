using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebLearning.ApiIntegration.Services;
using WebLearning.Contract.Dtos.BookingCalender.Room;

namespace WebLearning.AppUser.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly INotyfService _notyf;
        public RoomController(IRoomService roomService, INotyfService notyf)
        {
            _roomService = roomService;
            _notyf = notyf;
        }
        [Route("danh-sach-phong")]
        public async Task<IEnumerable<RoomDto>> Index()
        {
            var a = await _roomService.GetAllRooms();

            return a;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoom(CreateRoomDto createRoomDto)
        {
            var a = _roomService.InsertRoom(createRoomDto);

            if (a == null) { _notyf.Error("Thêm không thành công"); return Redirect("/admin.html"); };

            _notyf.Success("Thêm thành công");

            return Redirect("/admin.html");
        }
    }
}
