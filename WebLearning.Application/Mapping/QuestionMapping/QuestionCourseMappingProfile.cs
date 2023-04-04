using AutoMapper;
using WebLearning.Contract.Dtos.Question;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.QuestionMapping
{
    public class QuestionCourseMappingProfile : Profile
    {
        public QuestionCourseMappingProfile()
        {
            CreateMap<QuestionFinal, QuestionCourseDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                                .ForMember(dest => dest.QuizCourseId, opt => opt.MapFrom(src => src.QuizCourseId))
                                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                                .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Point))
                                                .ForPath(des => des.CorrectAnswerCourseDtos, opt => opt.MapFrom(src => src.CorrectAnswers))
                                                .ForPath(des => des.OptionCourseDtos, opt => opt.MapFrom(src => src.Options))
                                                .ForPath(dest => dest.QuizCourseDto, opt => opt.MapFrom(src => src.QuizCourse))
                                                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                                ;
            CreateMap<CreateQuestionCourseDto, QuestionFinal>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                    .ForMember(dest => dest.QuizCourseId, opt => opt.MapFrom(src => src.QuizCourseId))
                                    .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                    .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Point))
                                    .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                                                                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
;
            CreateMap<UpdateQuestionCourseDto, QuestionFinal>()
                        .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Point))
                        .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                                                        .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
;
        }
    }
}
