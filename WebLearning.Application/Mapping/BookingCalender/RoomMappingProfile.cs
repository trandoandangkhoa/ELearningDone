using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLearning.Contract.Dtos.BookingCalender;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.BookingCalender
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile() { 

            CreateMap<Room,RoomDto>();
        }
    }
}
