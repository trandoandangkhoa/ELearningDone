using AutoMapper;
using WebLearning.Contract.Dtos.CorrectAnswerLession;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.CorrectAnswerMappingProfile
{
    public class CorrectAnswerLessionMappingProfile : Profile
    {
        public CorrectAnswerLessionMappingProfile()
        {
            CreateMap<CorrectAnswerLession, CorrectAnswerLessionDto>();

            CreateMap<CorrectAnswerLessionDto, CorrectAnswerLession>();

            CreateMap<CreateCorrectAnswerLessionDto, CorrectAnswerLession>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer))
                                    .ForMember(dest => dest.OptionLessionId, opt => opt.MapFrom(src => src.OptionLessionId))
                                    .ForMember(dest => dest.QuestionLessionId, opt => opt.MapFrom(src => src.QuestionLessionId));
            CreateMap<UpdateCorrectAnswerLessionDto, CorrectAnswerLession>()
                        .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer));
        }
    }
}
