using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLearning.Contract.Dtos.Assets.Category
{
    public class CreateAssetCategoryDto
    {
        public Guid Id { get; set; }

        public string CatCode { get; set; }

        public string Name { get; set; }
    }
}
