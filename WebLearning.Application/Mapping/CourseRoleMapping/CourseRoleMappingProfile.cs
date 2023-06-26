using AutoMapper;
using WebLearning.Contract.Dtos.CourseRole;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.CourseRoleMapping
{
    public class CourseRoleMappingProfile : Profile
    {
        public CourseRoleMappingProfile()
        {

            CreateMap<CourseRole, CourseRoleDto>().ForPath(dest => dest.CourseDto, opt => opt.MapFrom(src => src.Course))
                                                    .ForPath(dest => dest.CourseDto.LessionDtos, opt => opt.MapFrom(src => src.Course.Lessions))
                                                  .ForPath(dest => dest.RoleDto, opt => opt.MapFrom(src => src.Role));
            //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //.ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
            //.ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            //.ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
            //.ForMember(dest => dest.CourseDto, opt => opt.MapFrom(src => src.Course))
            //.ForMember(dest => dest.RoleDto, opt => opt.MapFrom(src => src.Role));



            CreateMap<CreateCourseRoleDto, CourseRole>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                    .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                    .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                    .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName));

            CreateMap<UpdateCourseRoleDto, CourseRole>().ForMember(dest => dest.NumWatched, opt => opt.MapFrom(src => src.NumWatched));

        }
    }
}
