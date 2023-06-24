using AutoMapper;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsMovedStatusMappingProfile : Profile
    {
        public AssetsMovedStatusMappingProfile()
        {
            CreateMap<AssetsMovedStatus, AssetsMovedStatusDto>().ForPath(dest => dest.AssetsMovedDtos, opt => opt.MapFrom(src => src.AssetsMoved));
        }
    }
}
