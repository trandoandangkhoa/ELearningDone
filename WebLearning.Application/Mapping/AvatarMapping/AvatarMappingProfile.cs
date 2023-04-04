using AutoMapper;
using WebLearning.Contract.Dtos.Avatar;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AvatarMapping
{
    public class AvatarMappingProfile : Profile
    {
        public AvatarMappingProfile()
        {
            CreateMap<Avatar, AvatarDto>();
            CreateMap<CreateAvatarDto, Avatar>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                    .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                                    .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath))
                                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                    .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.FileSize))
                                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated));
            CreateMap<UpdateAvatarDto, Avatar>()
                                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                    .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImagePath))
                                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                    .ForMember(dest => dest.FileSize, opt => opt.MapFrom(src => src.FileSize))
                                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated));

        }
    }
}
