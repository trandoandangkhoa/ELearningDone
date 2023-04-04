using AutoMapper;
using WebLearning.Contract.Dtos.OptionMonthly;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.OptionMapping
{
    public class OptionMonthlyMappingProfile : Profile
    {
        public OptionMonthlyMappingProfile()
        {
            CreateMap<OptionMonthly, OptionMonthlyDto>();

            CreateMap<OptionMonthlyDto, OptionMonthly>();


            CreateMap<CreateOptionMonthlyDto, OptionMonthly>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                    .ForMember(dest => dest.QuestionMonthlyId, opt => opt.MapFrom(src => src.QuestionMonthlyId));
            CreateMap<UpdateOptionMonthlyDto, OptionMonthly>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
