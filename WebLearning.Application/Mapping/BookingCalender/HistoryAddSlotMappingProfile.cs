using AutoMapper;
using WebLearning.Contract.Dtos.BookingCalender.HistoryAddSlot;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.BookingCalender
{
    public class HistoryAddSlotMappingProfile : Profile
    {
        public HistoryAddSlotMappingProfile()
        {
            CreateMap<CreateHistoryAddSlotDto, HistoryAddSlot>();
            CreateMap<UpdateHistoryAddSlotDto, HistoryAddSlot>().ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.DescStatus));

            CreateMap<HistoryAddSlot, HistoryAddSlotDto>();

        }
    }
}
