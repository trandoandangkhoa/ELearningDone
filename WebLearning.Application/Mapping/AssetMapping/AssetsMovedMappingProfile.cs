using AutoMapper;
using WebLearning.Contract.Dtos.Assets.Moved;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsMovedMappingProfile : Profile
    {
        public AssetsMovedMappingProfile()
        {
            CreateMap<CreateAssetsMovedDto, AssetMoved>();
            CreateMap<AssetMoved, AssetsMovedDto>().ForMember(dest => dest.AssestsId, opt => opt.MapFrom(src => src.AssestsId))
                                        .ForPath(dest => dest.AssetsMovedStatusDto, opt => opt.MapFrom(src => src.AssetsMovedStatus));
            CreateMap<AssetMovedHistory, AssetMovedHistoryDto>();

            CreateMap<UpdateAssetsMovedDto, AssetMoved>();
            CreateMap<CreateAssetMovedHistoryDto, AssetMovedHistory>();

            CreateMap<AssetMoved, UpdateAssetsMovedDto>();
            CreateMap<AssetMoved, CreateAssetsMovedDto>();

        }
    }
}
