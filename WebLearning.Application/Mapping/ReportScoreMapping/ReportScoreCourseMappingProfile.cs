using AutoMapper;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.ReportScoreMapping
{
    public class ReportScoreCourseMappingProfile : Profile
    {
        public ReportScoreCourseMappingProfile()
        {
            CreateMap<ReportScoreCourseDto, ReportUserScoreFinal>();
            CreateMap<ReportUserScoreFinal, ReportScoreCourseDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.QuizCourseId, opt => opt.MapFrom(src => src.QuizCourseId))
                                    .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                                    .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate))
                                    .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore))
                                    .ForMember(dest => dest.Passed, opt => opt.MapFrom(src => src.Passed))
                                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                                    .ForMember(dest => dest.QuizCourseDtos, opt => opt.MapFrom(src => src.QuizCourses))
                                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                                    .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress));

            CreateMap<CreateReportScoreCourseDto, ReportUserScoreFinal>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                        .ForMember(dest => dest.QuizCourseId, opt => opt.MapFrom(src => src.QuizCourseId))
                        .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate))
                        .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore))
                        .ForMember(dest => dest.Passed, opt => opt.MapFrom(src => src.Passed))
                        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                                    .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress));



        }
    }
}
