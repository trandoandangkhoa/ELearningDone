using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Contract.Dtos.Assets.SubCategory;
using WebLearning.Contract.Dtos.Role;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AssetMapping
{
    public class AssetsSubCategoryMappingProfile : Profile
    {
        public AssetsSubCategoryMappingProfile() 
        {
            CreateMap<CreateAssetsSubCategoryDto, AssetsSubCategory>();
            CreateMap<AssetsSubCategory, AssetsSubCategoryDto>()
                                    .ForPath(dest => dest.AssetsCategoryDto, opt => opt.MapFrom(src => src.AssetsCategory));
            CreateMap<UpdateAssetsSubCategoryDto, AssetsSubCategory>();

        }
    }
}
