using AutoMapper;
using WebLearning.Contract.Dtos.CorrectAnswerMonthly;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.CorrectAnswerMappingProfile
{
    public class CorrectAnswerMonthlyMappingProfile : Profile
    {
        public CorrectAnswerMonthlyMappingProfile()
        {
            CreateMap<CorrectAnswerMonthly, CorrectAnswerMonthlyDto>();
            CreateMap<CorrectAnswerMonthlyDto, CorrectAnswerMonthly>();

            CreateMap<CreateCorrectAnswerMonthlyDto, CorrectAnswerMonthly>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer))
                                    .ForMember(dest => dest.OptionMonthlyId, opt => opt.MapFrom(src => src.OptionMonthlyId))
                                    .ForMember(dest => dest.QuestionMonthlyId, opt => opt.MapFrom(src => src.QuestionMonthlyId));
            CreateMap<UpdateCorrectAnswerMonthlyDto, CorrectAnswerMonthly>()
                        .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer));
        }
    }
}
