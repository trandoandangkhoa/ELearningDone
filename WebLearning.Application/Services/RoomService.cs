using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using WebLearning.Application.Helper;
using WebLearning.Application.Ultities;
using WebLearning.Contract.Dtos.BookingCalender.Room;
using WebLearning.Domain.Entites;
using WebLearning.Persistence.ApplicationContext;

namespace WebLearning.Application.Services
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetRoom();
        Task<RoomDto> GetRoomByid(int id);
        Task InsertRoom(CreateRoomDto createRoomDto);
        Task DeleteRoom(int id);
        Task UpdateRoom(UpdateRoomDto updateRoomDto, int id);

    }
    public class RoomService : IRoomService
    {
        private readonly WebLearningContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public RoomService(WebLearningContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task DeleteRoom(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id);

            _context.Rooms.Remove(room);

             _context.SaveChanges();
        }


        public async Task<IEnumerable<RoomDto>> GetRoom()
        {
            var room = await _context.Rooms.OrderBy(x => x.Id).AsNoTracking().ToListAsync();
            var roomDto = _mapper.Map<List<RoomDto>>(room);
            return roomDto;

        }

        public async Task<RoomDto> GetRoomByid(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(x => x.Id.Equals(id));

            return _mapper.Map<RoomDto>(room);
        }
        public async Task InsertRoom(CreateRoomDto createRoomDto)
        {
            Room room = _mapper.Map<Room>(createRoomDto);

            if (_context.Rooms.Any(x => x.Name.Equals(createRoomDto.Name)) == false)
            {

                _context.Add(room);
                await _context.SaveChangesAsync();
            }
            return;
        }

        public async Task UpdateRoom(UpdateRoomDto updateRoomDto, int id)
        {
            var item = await _context.Rooms.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (item != null)
            {

                _context.Rooms.Update(_mapper.Map(updateRoomDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
