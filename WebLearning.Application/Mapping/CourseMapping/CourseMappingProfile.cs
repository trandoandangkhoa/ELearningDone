using AutoMapper;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.ImageCourse;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.CourseMapping
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {

            CreateMap<CourseImageVideo, CourseImageDto>()/*.ForPath(dest => dest.CourseDto, opt => opt.MapFrom(src => src.Course))*/;

            CreateMap<Course, CourseDto>()
                    .ForPath(dest => dest.LessionDtos, opt => opt.MapFrom(src => src.Lessions))
                    .ForPath(dest => dest.QuizCourseDto, opt => opt.MapFrom(src => src.QuizCourse))
                    .ForPath(dest => dest.CourseRoleDtos, opt => opt.MapFrom(src => src.CourseRoles))
                    .ForPath(dest => dest.CourseImageVideoDto, opt => opt.MapFrom(src => src.CourseImageVideo));
            //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //.ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
            ////.ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            //.ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
            //.ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
            //.ForMember(dest => dest.CompletedTime, opt => opt.MapFrom(src => src.CompletedTime))
            ////.ForPath(dest => dest.roleDto, opt => opt.MapFrom(src => src.Role))
            //.ForPath(dest => dest.LessionDtos, opt => opt.MapFrom(src => src.Lessions))
            //.ForPath(dest => dest.QuizCourseDto, opt => opt.MapFrom(src => src.QuizCourse))
            //.ForPath(dest => dest.CourseRoleDtos, opt => opt.MapFrom(src => src.CourseRoles));


            CreateMap<CreateCourseDto, Course>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                   .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))

                                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                                .ForMember(dest => dest.Notify, opt => opt.MapFrom(src => src.Notify))
                                                .ForMember(dest => dest.DescNotify, opt => opt.MapFrom(src => src.DescNotify))

                                                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias));


            CreateMap<UpdateCourseDto, Course>()
                                                .ForMember(x => x.Id, opt => opt.Ignore())
                                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                                .ForMember(dest => dest.Notify, opt => opt.MapFrom(src => src.Notify))
                                                .ForMember(dest => dest.DescNotify, opt => opt.MapFrom(src => src.DescNotify))
                                                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                                .ForMember(dest => dest.TotalWatched, opt => opt.MapFrom(src => src.TotalWatched));


            CreateMap<UpdateTotalWatched, Course>().ForMember(dest => dest.TotalWatched, opt => opt.MapFrom(src => src.TotalWatched))
;
        }
    }
}
