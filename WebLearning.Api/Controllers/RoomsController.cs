using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ApiBase
    {
        private readonly WebLearningContext _context;

        public RoomsController(WebLearningContext context)
        {
            _context = context;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _context.Rooms.ToListAsync();

        }
        //public Object FromLocation()
        //{
        //    return (_context.Rooms.Select(x => new
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //    }).ToList().Where(x => x.Name != null));
        //}
        [HttpGet("usergetrooms")]
        public async Task<ActionResult<IEnumerable<Room>>> UserGetRooms()
        {
            return await _context.Rooms.ToListAsync();

        }
        [HttpGet("managergetrooms")]
        public async Task<ActionResult<IEnumerable<Room>>> ManagerGetRooms()
        {
            return await _context.Rooms.ToListAsync();

        }
        // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetDoctor(int id)
        {
            var doctor = await _context.Rooms.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return doctor;
        }

        // PUT: api/Doctors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, Room doctor)
        {
            Result rs = new();
            if (id != doctor.Id)
            {
                rs.message = "Không tìm thấy mã phòng";
                return Ok(rs);
            }

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                rs.message = "Cập nhật thành công";
                return Ok(rs);

            }
            catch (Exception ex)
            {
                if (!DoctorExists(id))
                {
                    rs.message = "Không tìm thấy phòng";
                    return Ok(rs);
                }
                else
                {
                    rs.message = ex.Message;
                    return Ok(rs);
                }
            }

        }

        // POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Room>> PostDoctor(Room doctor)
        {
            Result rs = new();
            _context.Rooms.Add(doctor);
            await _context.SaveChangesAsync();

            rs.message = "Tạo mới phòng thành công!";
            return Ok(rs);
        }

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {

            var doctor = await _context.Rooms.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Rooms.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DoctorExists(int id)
        {

            return _context.Rooms.Any(e => e.Id == id);
        }

    }
}
