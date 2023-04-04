using AutoMapper;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.HistorySubmitMapping
{
    public class HistorySubmitLessionMappingProfile : Profile
    {
        public HistorySubmitLessionMappingProfile()
        {
            CreateMap<HistorySubmitLession, HistorySubmitLessionDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                            .ForMember(dest => dest.QuizLessionId, opt => opt.MapFrom(src => src.QuizLessionId))
                                            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                                            .ForMember(dest => dest.DateCompoleted, opt => opt.MapFrom(src => src.DateCompoleted))
                                            .ForMember(dest => dest.Submit, opt => opt.MapFrom(src => src.Submit))
                                            .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore))
                                            .ForPath(dest => dest.QuizlessionDtos, opt => opt.MapFrom(src => src.QuizLessions));

            CreateMap<CreateHistorySubmitLessionDto, HistorySubmitLession>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                            .ForMember(dest => dest.QuizLessionId, opt => opt.MapFrom(src => src.QuizLessionId))
                                            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                                            .ForMember(dest => dest.DateCompoleted, opt => opt.MapFrom(src => src.DateCompoleted))
                                            .ForMember(dest => dest.Submit, opt => opt.MapFrom(src => src.Submit))
                                            .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore));

            CreateMap<UpdateHistorySubmitLessionDto, HistorySubmitLession>()
                                            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                                            .ForMember(dest => dest.DateCompoleted, opt => opt.MapFrom(src => src.DateCompoleted))
                                            .ForMember(dest => dest.Submit, opt => opt.MapFrom(src => src.Submit))
                                            .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore));
        }
    }
}
