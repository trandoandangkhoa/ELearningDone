using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Contract.Dtos.Assets.Department;
using WebLearning.Contract.Dtos.Assets.Status;

namespace WebLearning.Contract.Dtos.Assets
{
    public class CheckBox
    {
        public List<AssetsCategoryDto> AssetsCategoryDtos { get; set; }
        public List<AssetsDepartmentDto> AssetsDepartmentDtos { get; set;}
        public List<AssetsStatusDto> AssetsStatusDtos { get; set; }
    }
    
}
