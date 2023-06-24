using AutoMapper;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsMappingProfile : Profile
    {
        public AssetsMappingProfile()
        {
            CreateMap<CreateAssetsDto, Assests>();

            CreateMap<Assests, AssetsDto>().ForPath(dest => dest.AssetsStatusDto, opt => opt.MapFrom(src => src.AssetsStatus))
                                        .ForPath(dest => dest.AssetsDepartmentDto, opt => opt.MapFrom(src => src.AssetsDepartment))
                                        .ForPath(dest => dest.AssetsCategoryDto, opt => opt.MapFrom(src => src.AssetsCategory))
                                        .ForPath(dest => dest.AssetsMovedsDto, opt => opt.MapFrom(src => src.AssetsMoveds));
            CreateMap<UpdateAssetsDto, Assests>();

        }
    }
}
