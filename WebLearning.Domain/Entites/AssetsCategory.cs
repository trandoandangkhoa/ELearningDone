using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLearning.Domain.Entites
{
    public class AssetsCategory
    {
        [Key]
        public Guid Id { get; set; }

        public string CatCode { get;set; }

        public string Name { get; set; }

        public ICollection<AssetsSubCategory> SubCategories { get; set; }

        public ICollection<Assests> Assests { get; set; }

    }
}
