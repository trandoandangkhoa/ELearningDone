using AutoMapper;
using WebLearning.Contract.Dtos.AnswerLession;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AnswerMapping
{
    public class AnswerLessionMappingProfile : Profile
    {
        public AnswerLessionMappingProfile()
        {
            CreateMap<AnswerLession, AnswerLessionDto>()
                                                .ForMember(dest => dest.QuizLessionId, opt => opt.MapFrom(src => src.QuizLessionId))
                                                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionLessionId))
                                                .ForPath(dest => dest.Question, opt => opt.MapFrom(src => src.Question));

            CreateMap<CreateAnswerLessionDto, AnswerLession>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.QuizLessionId, opt => opt.MapFrom(src => src.QuizLessionId))
                                    .ForMember(dest => dest.QuestionLessionId, opt => opt.MapFrom(src => src.QuestionId))
                                    .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                                    .ForMember(dest => dest.CheckBoxId, opt => opt.MapFrom(src => src.CheckBoxId))
                                    .ForMember(dest => dest.Checked, opt => opt.MapFrom(src => src.Checked))
                                    .ForMember(dest => dest.OwnAnswer, opt => opt.MapFrom(src => src.OwnAnswer));

            CreateMap<UpdateAnswerLessionDto, AnswerLession>()
                        .ForMember(dest => dest.CheckBoxId, opt => opt.MapFrom(src => src.CheckBoxId))
                        .ForMember(dest => dest.Checked, opt => opt.MapFrom(src => src.Checked))
                        .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName))
                        .ForMember(dest => dest.OwnAnswer, opt => opt.MapFrom(src => src.OwnAnswer));

        }
    }
}
