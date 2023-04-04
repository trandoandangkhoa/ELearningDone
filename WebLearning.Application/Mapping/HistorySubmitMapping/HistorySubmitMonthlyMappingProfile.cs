using AutoMapper;
using WebLearning.Contract.Dtos.HistorySubmit;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.HistorySubmitMapping
{
    public class HistorySubmitMonthlyMappingProfile : Profile
    {
        public HistorySubmitMonthlyMappingProfile()
        {
            CreateMap<HistorySubmitMonthly, HistorySubmitMonthlyDto>();

            CreateMap<CreateHistorySubmitMonthlyDto, HistorySubmitMonthly>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                            .ForMember(dest => dest.QuizMonthlyId, opt => opt.MapFrom(src => src.QuizMonthlyId))
                                            .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                                            .ForMember(dest => dest.DateCompoleted, opt => opt.MapFrom(src => src.DateCompoleted))
                                            .ForMember(dest => dest.Submit, opt => opt.MapFrom(src => src.Submit))
                                            .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.TotalScore));

        }
    }
}
