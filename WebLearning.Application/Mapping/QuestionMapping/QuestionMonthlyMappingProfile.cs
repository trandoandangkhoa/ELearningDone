using AutoMapper;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.QuestionMapping
{
    public class QuestionMonthlyMappingProfile : Profile
    {
        public QuestionMonthlyMappingProfile()
        {
            CreateMap<QuestionMonthly, QuestionMonthlyDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                                .ForMember(dest => dest.QuizMonthlyId, opt => opt.MapFrom(src => src.QuizMonthlyId))
                                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                                .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Point))
                                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                                 .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))

                                                .ForPath(dest => dest.QuizMonthlyDto, opt => opt.MapFrom(src => src.QuizMonthly))
                                                .ForPath(des => des.CorrectAnswerMonthlyDtos, opt => opt.MapFrom(src => src.CorrectAnswers))
                                                .ForPath(des => des.OptionMonthlyDtos, opt => opt.MapFrom(src => src.Options));
            CreateMap<CreateQuestionMonthlyDto, QuestionMonthly>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                    .ForMember(dest => dest.QuizMonthlyId, opt => opt.MapFrom(src => src.QuizMonthlyId))
                                    .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                    .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Point))
                                    .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
;
            CreateMap<UpdateQuestionMonthlyDto, QuestionMonthly>()
                        .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Point))
                        .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                        .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
;
        }
    }
}
