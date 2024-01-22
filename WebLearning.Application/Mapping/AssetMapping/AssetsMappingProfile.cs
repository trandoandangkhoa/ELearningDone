using AutoMapper;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsMappingProfile : Profile
    {
        public AssetsMappingProfile()
        {
            CreateMap<CreateAssetsDto, Asset>();

            CreateMap<Asset, CreateAssetsDto>();

            CreateMap<Asset, AssetsDto>().ForPath(dest => dest.AssetsStatusDto, opt => opt.MapFrom(src => src.AssetsStatus))
                                        .ForPath(dest => dest.AssetsDepartmentDto, opt => opt.MapFrom(src => src.AssetsDepartment))
                                        .ForPath(dest => dest.AssetsCategoryDto, opt => opt.MapFrom(src => src.AssetsCategory))
                                        .ForPath(dest => dest.AssetsMovedsDto, opt => opt.MapFrom(src => src.AssetsMoveds))
                                        .ForPath(dest => dest.AssetsSupplierDto, opt => opt.MapFrom(src => src.AssetsSupplier))
                                        .ForPath(dest => dest.AssetsRepairedDtos, opt => opt.MapFrom(src => src.AssetsRepaireds));
            CreateMap<UpdateAssetsDto, Asset>();

            CreateMap<Asset, UpdateAssetsDto>();

        }
    }
}
