using AutoMapper;
using WebLearning.Contract.Dtos.Assets.Status;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsStatusMappingProfile : Profile
    {
        public AssetsStatusMappingProfile()
        {
            CreateMap<CreateAssetsStatusDto, AssetsStatus>();
            CreateMap<AssetsStatus, AssetsStatusDto>().ForPath(dest => dest.AssetsDtos, opt => opt.MapFrom(src => src.Assests));
            CreateMap<UpdateAssetsStatusDto, AssetsStatus>();

        }
    }
}
