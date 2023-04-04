using AutoMapper;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.HistorySubmitMapping
{
    public class HistorySubmitCourseMappingProfile : Profile
    {
        public HistorySubmitCourseMappingProfile()
        {
            CreateMap<HistorySubmitFinal, HistorySubmitCourseDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                            .ForMember(dest => dest.QuizCourseId, opt => opt.MapFrom(src => src.QuizCourseId))
                                            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                                            .ForMember(dest => dest.DateCompoleted, opt => opt.MapFrom(src => src.DateCompoleted))
                                            .ForMember(dest => dest.Submit, opt => opt.MapFrom(src => src.Submit))
                                            .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore))
                                            .ForPath(dest => dest.QuizCourseDtos, opt => opt.MapFrom(src => src.QuizCourses));

            CreateMap<CreateHistorySubmitCourseDto, HistorySubmitFinal>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                            .ForMember(dest => dest.QuizCourseId, opt => opt.MapFrom(src => src.QuizCourseId))
                                            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                                            .ForMember(dest => dest.DateCompoleted, opt => opt.MapFrom(src => src.DateCompoleted))
                                            .ForMember(dest => dest.Submit, opt => opt.MapFrom(src => src.Submit))
                                            .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore));

        }
    }
}
