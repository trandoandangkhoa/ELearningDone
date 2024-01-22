using WebLearning.Contract.Dtos.Assets.Category;
using WebLearning.Contract.Dtos.Assets.Department;
using WebLearning.Contract.Dtos.Assets.Status;

namespace WebLearning.Contract.Dtos.Assets
{
    public class CheckBox
    {
        public List<AssetsCategoryDto> AssetsCategoryDtos { get; set; }
        public List<AssetsDepartmentDto> AssetsDepartmentLocationDtos { get; set; }

        public List<AssetsDepartmentDto> AssetsDepartmentDtos { get; set; }

        public List<AssetsDepartmentDto> AssetsDepartmentDtosMoving { get; set; }

        public List<AssetsStatusDto> AssetsStatusDtos { get; set; }
    }

}
