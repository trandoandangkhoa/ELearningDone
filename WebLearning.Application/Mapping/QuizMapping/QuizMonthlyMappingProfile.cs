using AutoMapper;
using WebLearning.Contract.Dtos.Quiz;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.QuizMapping
{
    public class QuizMonthlyMappingProfile : Profile
    {
        public QuizMonthlyMappingProfile()
        {
            CreateMap<QuizMonthly, QuizMonthlyDto>().ForPath(dest => dest.QuestionMonthlyDtos, opt => opt.MapFrom(src => src.QuestionMonthlies))
                                                    .ForPath(dest => dest.RoleDto, opt => opt.MapFrom(src => src.Role));
            //CreateMap<AnswerMonthly, AnswerMonthlyDto>();

            //CreateMap<AnswerMonthlyDto, AnswerMonthly>();


            CreateMap<CreateQuizMonthlyDto, QuizMonthly>().ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.ID))
                                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                                                .ForMember(dest => dest.Notify, opt => opt.MapFrom(src => src.Notify))
                                                .ForMember(dest => dest.DescNotify, opt => opt.MapFrom(src => src.DescNotify)).ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                .ForMember(dest => dest.TimeToDo, opt => opt.MapFrom(src => src.TimeToDo))
                                .ForMember(dest => dest.ScorePass, opt => opt.MapFrom(src => src.ScorePass))
                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias));


            CreateMap<UpdateQuizMonthlyDto, QuizMonthly>()
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
