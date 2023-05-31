using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLearning.Contract.Dtos.Assets.SubCategory;

namespace WebLearning.Contract.Dtos.Assets.Category
{
    public class AssetsCategoryDto
    {
        public Guid Id { get; set; }

        public string CatCode { get; set; }

        public string Name { get; set; }

        public ICollection<AssetsSubCategoryDto> AssetsSubCategoryDtos { get; set; }

        public ICollection<AssetsDto> AssetsDtos { get; set; }
    }
}
