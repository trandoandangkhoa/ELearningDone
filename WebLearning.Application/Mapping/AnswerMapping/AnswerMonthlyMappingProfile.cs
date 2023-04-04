using AutoMapper;
using WebLearning.Contract.Dtos.AnswerMonthly;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AnswerMapping
{
    public class AnswerMonthlyMappingProfile : Profile
    {
        public AnswerMonthlyMappingProfile()
        {
            CreateMap<AnswerMonthly, AnswerMonthlyDto>()
                                                                .ForMember(dest => dest.QuizMonthlyId, opt => opt.MapFrom(src => src.QuizMonthlyId))
                                                .ForMember(dest => dest.QuestionMonthlyId, opt => opt.MapFrom(src => src.QuestionMonthlyId))
                                                .ForPath(dest => dest.QuestionMonthlyDto, opt => opt.MapFrom(src => src.QuestionMonthly));

            CreateMap<CreateAnswerMonthlyDto, AnswerMonthly>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.QuizMonthlyId, opt => opt.MapFrom(src => src.QuizMonthlyId))
                                    .ForMember(dest => dest.QuestionMonthlyId, opt => opt.MapFrom(src => src.QuestionMonthlyId))
                                    .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                                    .ForMember(dest => dest.OwnAnswer, opt => opt.MapFrom(src => src.OwnAnswer));

            CreateMap<UpdateAnswerMonthlyDto, AnswerMonthly>()
                        .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                        .ForMember(dest => dest.OwnAnswer, opt => opt.MapFrom(src => src.OwnAnswer));

        }
    }
}
