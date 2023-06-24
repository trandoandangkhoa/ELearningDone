using AutoMapper;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsCategoryMappingProfile : Profile
    {
        public AssetsCategoryMappingProfile()
        {
            CreateMap<CreateAssetsCategoryDto, AssetsCategory>();
            CreateMap<AssetsCategory, AssetsCategoryDto>().ForPath(dest => dest.AssetsDtos, opt => opt.MapFrom(src => src.Assests));
            CreateMap<UpdateAssetsCategoryDto, AssetsCategory>();

        }
    }
}
