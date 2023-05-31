using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLearning.Contract.Dtos.Assets;
using WebLearning.Contract.Dtos.Assets.Department;
using WebLearning.Contract.Dtos.Role;
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
                                        .ForPath(dest => dest.AssetsCategoryDto, opt => opt.MapFrom(src => src.AssetsCategory));
            CreateMap<UpdateAssetsDto, Assests>();

        }
    }
}
