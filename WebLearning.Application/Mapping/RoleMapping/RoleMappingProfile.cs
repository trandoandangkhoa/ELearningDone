using AutoMapper;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.RoleMapping
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<CreateRoleDto, Role>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                                            .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<RoleDto, Role>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                    .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                                    .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias));
            CreateMap<UpdateRoleDto, Role>()
                                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias));

            CreateMap<Role, RoleDto>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                    .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleName))
                                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                                    .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias));



        }


    }
}
