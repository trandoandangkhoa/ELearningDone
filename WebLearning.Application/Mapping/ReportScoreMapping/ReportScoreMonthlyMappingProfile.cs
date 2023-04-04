using AutoMapper;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.ReportScoreMapping
{
    public class ReportScoreMonthlyMappingProfile : Profile
    {
        public ReportScoreMonthlyMappingProfile()
        {
            CreateMap<ReportScoreMonthlyDto, ReportUserScoreMonthly>();
            CreateMap<ReportUserScoreMonthly, ReportScoreMonthlyDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.QuizMonthlyId, opt => opt.MapFrom(src => src.QuizMonthlyId))
                                    .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                                    .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate))
                                    .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore))
                                    .ForMember(dest => dest.Passed, opt => opt.MapFrom(src => src.Passed))
                                    .ForMember(dest => dest.QuizMonthlyDtos, opt => opt.MapFrom(src => src.QuizMonthlies))
                                     .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            CreateMap<CreateReportScoreMonthlyDto, ReportUserScoreMonthly>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                        .ForMember(dest => dest.QuizMonthlyId, opt => opt.MapFrom(src => src.QuizMonthlyId))
                        .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate))
                        .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore))
                        .ForMember(dest => dest.Passed, opt => opt.MapFrom(src => src.Passed))
                        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                                    .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress));



        }
    }
}
