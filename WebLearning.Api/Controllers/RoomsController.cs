using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLearning.Application.Helper;
using WebLearning.Application.Services;
using WebLearning.Contract.Dtos;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Contract.Dtos.BookingCalender.Room;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ApiBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/Doctors
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StaffRole, AuthorizeRole.ManagerRole, AuthorizeRole.TeacherRole)]
        [HttpGet]
        public async Task<IEnumerable<RoomDto>> GetRooms()
        {
            return await _roomService.GetRoom();

        }

        [HttpGet("{id}")]
        [SecurityRole(AuthorizeRole.AdminRole, AuthorizeRole.StaffRole, AuthorizeRole.ManagerRole, AuthorizeRole.TeacherRole)]
        public async Task<ActionResult<RoomDto>> GetDoctor(int id)
        {
            var room = await _roomService.GetRoomByid(id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        // PUT: api/Doctors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, UpdateRoomDto updateRoomDto)
        {
            Result rs = new();
            try
            {
                await _roomService.UpdateRoom(updateRoomDto,id);
                rs.message = "Cập nhật thành công";
                return Ok(rs);

            }
            catch (Exception ex)
            {
                rs.message = ex.Message;
                return Ok(rs);
            }

        }

        // POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostDoctor(CreateRoomDto createRoomDto)
        {
            Result rs = new();
            await _roomService.InsertRoom(createRoomDto);

            rs.message = "Tạo mới phòng thành công!";
            return Ok(rs);
        }

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            try{
                var doctor = _roomService.DeleteRoom(id);
                return Ok();

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

        }

    }
}
