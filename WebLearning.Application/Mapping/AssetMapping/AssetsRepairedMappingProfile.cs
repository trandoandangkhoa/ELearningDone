using AutoMapper;
using WebLearning.Contract.Dtos.Assets.Repair;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsRepairedMappingProfile : Profile
    {
        public AssetsRepairedMappingProfile()
        {
            CreateMap<CreateAssetsRepairedDto, AssetsRepaired>();
            CreateMap<AssetsRepaired, AssetsRepairedDto>().ForPath(dest => dest.AssetsDto, opt => opt.MapFrom(src => src.Assests));
            CreateMap<UpdateAssetsRepairedDto, AssetsRepaired>();

        }
    }
}
