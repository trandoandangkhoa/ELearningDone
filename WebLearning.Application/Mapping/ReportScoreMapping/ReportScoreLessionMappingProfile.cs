using AutoMapper;
using WebLearning.Contract.Dtos.ReportScore;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.ReportScoreMapping
{
    public class ReportScoreLessionMappingProfile : Profile
    {
        public ReportScoreLessionMappingProfile()
        {
            CreateMap<ReportUserScore, ReportScoreLessionDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.QuizLessionId, opt => opt.MapFrom(src => src.QuizLessionId))
                                    .ForMember(dest => dest.LessionId, opt => opt.MapFrom(src => src.LessionId))
                                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                                    .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate))
                                    .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore))
                                    .ForMember(dest => dest.Passed, opt => opt.MapFrom(src => src.Passed))
                                    .ForMember(dest => dest.QuizlessionDtos, opt => opt.MapFrom(src => src.QuizLessions))
                                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                                    .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress));
            CreateMap<CreateReportScoreLessionDto, ReportUserScore>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.QuizLessionId, opt => opt.MapFrom(src => src.QuizLessionId))
                                    .ForMember(dest => dest.LessionId, opt => opt.MapFrom(src => src.LessionId))
                                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                                    .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate))
                                    .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore))
                                    .ForMember(dest => dest.Passed, opt => opt.MapFrom(src => src.Passed))
                                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                                    .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress));

            CreateMap<UpdateReportScoreLessionDto, ReportUserScore>()
                        .ForMember(dest => dest.QuizLessionId, opt => opt.MapFrom(src => src.QuizLessionId))
                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate))
                        .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore))
                        .ForMember(dest => dest.Passed, opt => opt.MapFrom(src => src.Passed))
                        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                        .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress));

        }
    }
}
