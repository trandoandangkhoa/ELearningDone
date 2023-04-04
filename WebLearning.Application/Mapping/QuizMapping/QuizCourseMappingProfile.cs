using AutoMapper;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.QuizMapping
{
    public class QuizCourseMappingProfile : Profile
    {
        public QuizCourseMappingProfile()
        {
            CreateMap<QuizCourse, QuizCourseDto>().ForPath(dest => dest.QuestionCourseDtos, opt => opt.MapFrom(src => src.QuestionFinals))
                                                    .ForPath(dest => dest.CourseDto, opt => opt.MapFrom(src => src.Course));

            //CreateMap<AnswerCourse, AnswerCourseDto>();

            //CreateMap<AnswerCourseDto, AnswerCourse>();


            CreateMap<CreateQuizCourseDto, QuizCourse>().ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                                .ForMember(dest => dest.Notify, opt => opt.MapFrom(src => src.Notify))
                                                .ForMember(dest => dest.DescNotify, opt => opt.MapFrom(src => src.DescNotify))
                                .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                .ForMember(dest => dest.TimeToDo, opt => opt.MapFrom(src => src.TimeToDo))
                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                .ForMember(dest => dest.ScorePass, opt => opt.MapFrom(src => src.ScorePass));


            CreateMap<UpdateQuizCourseDto, QuizCourse>()
                                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                                .ForMember(dest => dest.Notify, opt => opt.MapFrom(src => src.Notify))
                                                .ForMember(dest => dest.DescNotify, opt => opt.MapFrom(src => src.DescNotify)).ForMember(dest => dest.TimeToDo, opt => opt.MapFrom(src => src.TimeToDo))
                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                .ForMember(dest => dest.ScorePass, opt => opt.MapFrom(src => src.ScorePass));

        }
    }
}
