using AutoMapper;
using WebLearning.Contract.Dtos.CorrectAnswerCourse;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.CorrectAnswerMappingProfile
{
    public class CorrectAnswerCourseMappingProfile : Profile
    {
        public CorrectAnswerCourseMappingProfile()
        {
            CreateMap<CorrectAnswerCourse, CorrectAnswerCourseDto>();

            CreateMap<CorrectAnswerCourseDto, CorrectAnswerCourse>();

            CreateMap<CreateCorrectAnswerCourseDto, CorrectAnswerCourse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer))
                                    .ForMember(dest => dest.OptionCourseId, opt => opt.MapFrom(src => src.OptionCourseId))
                                    .ForMember(dest => dest.QuestionCourseId, opt => opt.MapFrom(src => src.QuestionCourseId));
            CreateMap<UpdateCorrectAnswerCourseDto, CorrectAnswerCourse>()
                        .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.CorrectAnswer));
        }
    }
}
