using AutoMapper;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.QuestionMapping
{
    public class QuestionLessionMappingProfile : Profile
    {
        public QuestionLessionMappingProfile()
        {
            CreateMap<QuestionLession, QuestionLessionDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                                .ForMember(dest => dest.QuizLessionId, opt => opt.MapFrom(src => src.QuizLessionId))
                                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                                .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Point))
                                                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))

                                                .ForPath(des => des.CorrectAnswerLessionDtos, opt => opt.MapFrom(src => src.CorrectAnswers))
                                                .ForPath(des => des.OptionLessionDtos, opt => opt.MapFrom(src => src.Options))
                                                .ForPath(dest => dest.QuizlessionDto, opt => opt.MapFrom(src => src.QuizLession));
            CreateMap<CreateQuestionLessionDto, QuestionLession>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                    .ForMember(dest => dest.QuizLessionId, opt => opt.MapFrom(src => src.QuizLessionId))
                                    .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                    .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Point))
                                    .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
;


            CreateMap<UpdateQuestionLessionDto, QuestionLession>()
                        .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Point))
                        .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                        .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
;
        }
    }
}
