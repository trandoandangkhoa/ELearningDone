using AutoMapper;
using WebLearning.Contract.Dtos.Assets.Status;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsStatusMappingProfile : Profile
    {
        public AssetsStatusMappingProfile()
        {
            CreateMap<CreateAssetsStatusDto, AssetStatus>();
            CreateMap<AssetStatus, AssetsStatusDto>().ForPath(dest => dest.AssetsDtos, opt => opt.MapFrom(src => src.Assests));
            CreateMap<UpdateAssetsStatusDto, AssetStatus>();

        }
    }
}
