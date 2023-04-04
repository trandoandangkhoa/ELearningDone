using AutoMapper;
using WebLearning.Contract.Dtos.OptionLession;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.OptionMapping
{
    public class OptionLessionMappingProfile : Profile
    {
        public OptionLessionMappingProfile()
        {
            CreateMap<OptionLession, OptionLessionDto>();

            CreateMap<OptionLessionDto, OptionLession>();

            CreateMap<CreateOptionLessionDto, OptionLession>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                    .ForMember(dest => dest.QuestionLessionId, opt => opt.MapFrom(src => src.QuestionLessionId));
            CreateMap<UpdateOptionLessionDto, OptionLession>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
