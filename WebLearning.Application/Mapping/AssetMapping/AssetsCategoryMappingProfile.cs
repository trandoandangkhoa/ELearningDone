using AutoMapper;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsCategoryMappingProfile : Profile
    {
        public AssetsCategoryMappingProfile()
        {
            CreateMap<CreateAssetsCategoryDto, AssetCategory>();
            CreateMap<AssetCategory, AssetsCategoryDto>().ForPath(dest => dest.AssetsDtos, opt => opt.MapFrom(src => src.Assests));
            CreateMap<UpdateAssetsCategoryDto, AssetCategory>();

        }
    }
}
