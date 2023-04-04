using AutoMapper;
using WebLearning.Contract.Dtos.OptionCourse;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.OptionMapping
{
    public class OptionCourseMappingProfile : Profile
    {
        public OptionCourseMappingProfile()
        {
            CreateMap<OptionCourse, OptionCourseDto>();

            CreateMap<OptionCourseDto, OptionCourse>();


            CreateMap<CreateOptionCourseDto, OptionCourse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                    .ForMember(dest => dest.QuestionFinalId, opt => opt.MapFrom(src => src.QuestionFinalId));
            CreateMap<UpdateOptionCourseDto, OptionCourse>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
