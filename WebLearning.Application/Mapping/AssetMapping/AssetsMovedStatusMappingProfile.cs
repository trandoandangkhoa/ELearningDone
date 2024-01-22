using AutoMapper;
using WebLearning.Contract.Dtos.Assets.Moved;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsMovedStatusMappingProfile : Profile
    {
        public AssetsMovedStatusMappingProfile()
        {
            CreateMap<AssetMovedStatus, AssetsMovedStatusDto>().ForPath(dest => dest.AssetsMovedDtos, opt => opt.MapFrom(src => src.AssetsMoved));
        }
    }
}
