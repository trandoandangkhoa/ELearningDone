using AutoMapper;
using WebLearning.Contract.Dtos.Assets.Supplier;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsSupplierMappingProfile : Profile
    {
        public AssetsSupplierMappingProfile()
        {
            CreateMap<CreateAssetsSupplierDto, AssetSupplier>();
            CreateMap<AssetSupplier, AssetsSupplierDto>().ForPath(dest => dest.AssetsDtos, opt => opt.MapFrom(src => src.Assests));
            CreateMap<UpdateAssetsSupplierDto, AssetSupplier>();

        }
    }
}
