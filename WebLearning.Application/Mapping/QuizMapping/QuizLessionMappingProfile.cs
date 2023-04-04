using AutoMapper;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.QuizMapping
{
    public class QuizLessionMappingProfile : Profile
    {
        public QuizLessionMappingProfile()
        {

            //CreateMap<QuizDto, Quiz>();
            //CreateMap<AnswerLession, AnswerLessionDto>();

            //CreateMap<AnswerLessionDto, AnswerLession>();


            CreateMap<QuizLession, QuizlessionDto>().ForPath(dest => dest.QuestionLessionDtos, opt => opt.MapFrom(src => src.QuestionLessions))
                                                    .ForPath(dest => dest.LessionDto, opt => opt.MapFrom(src => src.Lession))
                                                    .ForPath(dest => dest.LessionDto.CourseDto, opt => opt.MapFrom(src => src.Lession.Courses));

            CreateMap<CreateQuizLessionDto, QuizLession>().ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                .ForMember(dest => dest.LessionId, opt => opt.MapFrom(src => src.LessionId))
                                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                .ForMember(dest => dest.TimeToDo, opt => opt.MapFrom(src => src.TimeToDo))
                                .ForMember(dest => dest.SortItem, opt => opt.MapFrom(src => src.SortItem))
                                .ForMember(dest => dest.ScorePass, opt => opt.MapFrom(src => src.ScorePass))
                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                                .ForMember(dest => dest.Notify, opt => opt.MapFrom(src => src.Notify))
                                                .ForMember(dest => dest.DescNotify, opt => opt.MapFrom(src => src.DescNotify));


            CreateMap<UpdateQuizLessionDto, QuizLession>()
                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                                .ForMember(dest => dest.Notify, opt => opt.MapFrom(src => src.Notify))
                                                .ForMember(dest => dest.DescNotify, opt => opt.MapFrom(src => src.DescNotify)).ForMember(dest => dest.TimeToDo, opt => opt.MapFrom(src => src.TimeToDo))
                                .ForMember(dest => dest.ScorePass, opt => opt.MapFrom(src => src.ScorePass))
                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias));
        }
    }
}
