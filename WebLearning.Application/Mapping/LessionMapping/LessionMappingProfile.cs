using AutoMapper;
using WebLearning.Contract.Dtos.Lession;
using WebLearning.Contract.Dtos.LessionFileDocument;
using WebLearning.Contract.Dtos.VideoLession;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.LessionMapping
{
    public class LessionMappingProfile : Profile
    {
        public LessionMappingProfile()
        {
            CreateMap<Lession, LessionDto>()
                .ForPath(dest => dest.CourseDto, opt => opt.MapFrom(src => src.Courses))
                .ForPath(dest => dest.QuizlessionDtos, opt => opt.MapFrom(src => src.Quizzes))
                .ForPath(dest => dest.LessionFileDocumentDtos, opt => opt.MapFrom(src => src.OtherFileUploads))
                .ForPath(dest => dest.LessionVideoDtos, opt => opt.MapFrom(src => src.LessionVideoImages));

            CreateMap<LessionVideoImage, LessionVideoDto>();

            CreateMap<OtherFileUpload, LessionFileDocumentDto>();

            CreateMap<CreateLessionDto, Lession>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                    .ForMember(dest => dest.ShortDesc, opt => opt.MapFrom(src => src.ShortDesc))
                                    .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                    .ForMember(dest => dest.CourseId, opt => opt.MapFrom(src => src.CourseId))
                                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                    .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                    .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                                                .ForMember(dest => dest.Notify, opt => opt.MapFrom(src => src.Notify))
                                                .ForMember(dest => dest.DescNotify, opt => opt.MapFrom(src => src.DescNotify));


            CreateMap<UpdateLessionDto, Lession>()
                                                .ForMember(x => x.Id, opt => opt.Ignore())
                                                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                                                .ForMember(dest => dest.ShortDesc, opt => opt.MapFrom(src => src.ShortDesc))
                                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                                .ForMember(dest => dest.Alias, opt => opt.MapFrom(src => src.Alias))
                                                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                                                .ForMember(dest => dest.Notify, opt => opt.MapFrom(src => src.Notify))
                                                .ForMember(dest => dest.DescNotify, opt => opt.MapFrom(src => src.DescNotify));


        }

    }
}
