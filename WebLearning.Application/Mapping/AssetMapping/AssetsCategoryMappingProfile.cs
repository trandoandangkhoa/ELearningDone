using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsCategoryMappingProfile : Profile
    {
        public AssetsCategoryMappingProfile() 
        {
            CreateMap<CreateAssetCategoryDto, AssetsCategory>();
            CreateMap<AssetsCategory, AssetsCategoryDto>().ForPath(dest => dest.AssetsDtos, opt => opt.MapFrom(src => src.Assests))
                                    .ForPath(dest => dest.AssetsSubCategoryDtos, opt => opt.MapFrom(src => src.SubCategories));
            CreateMap<UpdateAssetsCategoryDto, AssetsCategory>();

        }
    }
}
