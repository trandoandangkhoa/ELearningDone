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
    public class AssetsDto
    {
        public string Id { get; set; }

        public string AssetId { get; set; }

        public string AssetName { get; set; }

        public Guid AssetsCategoryId { get; set; }

        public Guid AssetsSubCategoryId { get; set; }

        public int Quantity { get; set; }

        public Guid AssetsDepartmentId { get; set; }

        public string Customer { get; set; }
        public string Manager { get; set; }

        public int AssetsStatusId { get; set; }

        public DateTime DateUsed { get; set; }


        public DateTime DateChecked { get; set; }

        public string Spec { get; set; }

        public string Note { get; set; }

        public AssetsCategoryDto AssetsCategoryDto { get; set; }

        public AssetsDepartmentDto AssetsDepartmentDto { get; set; }

        public AssetsStatusDto AssetsStatusDto{ get; set; }
    }
}
