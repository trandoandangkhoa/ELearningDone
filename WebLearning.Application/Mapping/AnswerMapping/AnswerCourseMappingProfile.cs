using AutoMapper;
using WebLearning.Contract.Dtos.AnswerCourse;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AnswerMapping
{
    public class AnswerCourseMappingProfile : Profile
    {
        public AnswerCourseMappingProfile()
        {
            CreateMap<AnswerCourse, AnswerCourseDto>()
                                                                .ForMember(dest => dest.QuizCourseId, opt => opt.MapFrom(src => src.QuizCourseId))
                                                .ForMember(dest => dest.QuestionCourseId, opt => opt.MapFrom(src => src.QuestionCourseId))
                                                .ForPath(dest => dest.QuestionCourseDto, opt => opt.MapFrom(src => src.QuestionFinal));

            CreateMap<CreateAnswerCourseDto, AnswerCourse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.QuizCourseId, opt => opt.MapFrom(src => src.QuizCourseId))
                                    .ForMember(dest => dest.QuestionCourseId, opt => opt.MapFrom(src => src.QuestionCourseId))
                                    .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                                    .ForMember(dest => dest.OwnAnswer, opt => opt.MapFrom(src => src.OwnAnswer));

            CreateMap<UpdateAnswerCourseDto, AnswerCourse>()
                        .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                        .ForMember(dest => dest.OwnAnswer, opt => opt.MapFrom(src => src.OwnAnswer));

        }
    }
}
