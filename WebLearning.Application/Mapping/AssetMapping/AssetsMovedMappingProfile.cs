using AutoMapper;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsMovedMappingProfile : Profile
    {
        public AssetsMovedMappingProfile()
        {
            CreateMap<CreateAssetsMovedDto, AssetsMoved>();
            CreateMap<AssetsMoved, AssetsMovedDto>().ForMember(dest => dest.AssetsId, opt => opt.MapFrom(src => src.AssestsId))
                                        .ForPath(dest => dest.AssetsMovedStatusDto, opt => opt.MapFrom(src => src.AssetsMovedStatus));
            CreateMap<UpdateAssetsMovedDto, AssetsMoved>();


        }
    }
}
