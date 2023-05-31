using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLearning.Domain.Entites
{
    public class AssetsSubCategory
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AssetsCategoryId { get; set; }

        public string SubCode { get; set; }

        public string Name { get; set; }

        public AssetsCategory AssetsCategory { get; set; }
    }
}
