using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLearning.Contract.Dtos.Assets.Category;

namespace WebLearning.Contract.Dtos.Assets.SubCategory
{
    public class AssetsSubCategoryDto
    {
        public Guid Id { get; set; }
        public Guid AssetsCategoryId { get; set; }

        public string SubCode { get; set; }

        public string Name { get; set; }

        public AssetsCategoryDto AssetsCategoryDto { get; set; }
    }
}
