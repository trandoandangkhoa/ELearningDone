using AutoMapper;
using WebLearning.Contract.Dtos.Assets.Department;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsDepartmentMappingProfile : Profile
    {
        public AssetsDepartmentMappingProfile()
        {
            CreateMap<CreateAssetsDepartmentDto, AssetsDepartment>();
            CreateMap<AssetsDepartment, AssetsDepartmentDto>().ForPath(dest => dest.AssetsDto, opt => opt.MapFrom(src => src.Assests));
            CreateMap<UpdateAssetsDepartmentDto, AssetsDepartment>();

        }
    }
}
