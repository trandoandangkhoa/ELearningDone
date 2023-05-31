using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLearning.Contract.Dtos.Assets.SubCategory
{
    public class UpdateAssetsSubCategoryDto
    {
        public Guid AssetsCategoryId { get; set; }
        public string Name { get; set; }

    }
}
