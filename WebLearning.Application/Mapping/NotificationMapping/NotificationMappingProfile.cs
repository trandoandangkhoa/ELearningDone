using AutoMapper;
using WebLearning.Contract.Dtos.Course;
using WebLearning.Contract.Dtos.Notification;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.NotificationMapping
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {

            CreateMap<NotificationResponse, NotificationResponseDto>();

          
            CreateMap<CreateNotificationResponseDto, NotificationResponse>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.TargetNotificationId, opt => opt.MapFrom(src => src.TargetNotificationId))
                                    .ForMember(dest => dest.FatherAlias, opt => opt.MapFrom(src => src.FatherAlias))
                                    .ForMember(dest => dest.FatherTargetId, opt => opt.MapFrom(src => src.FatherTargetId))
                                    .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                                    .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                    .ForMember(dest => dest.TargetName, opt => opt.MapFrom(src => src.TargetName))
                                    .ForMember(dest => dest.TargetImagePath, opt => opt.MapFrom(src => src.TargetImagePath))
                                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                    .ForMember(dest => dest.Notify, opt => opt.MapFrom(src => src.Notify))
                                    .ForMember(dest => dest.DescNotify, opt => opt.MapFrom(src => src.DescNotify))
                                    .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName));
            CreateMap<UpdateNotificationResponseDto, NotificationResponse>()
                                    //.ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                    //.ForMember(dest => dest.TargetName, opt => opt.MapFrom(src => src.TargetName))
                                    //.ForMember(dest => dest.TargetImagePath, opt => opt.MapFrom(src => src.TargetImagePath))
                                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                    .ForMember(dest => dest.Notify, opt => opt.MapFrom(src => src.Notify));
            //.ForMember(dest => dest.DescNotify, opt => opt.MapFrom(src => src.DescNotify))
            //.ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName));
            ;


        }
    }
}
