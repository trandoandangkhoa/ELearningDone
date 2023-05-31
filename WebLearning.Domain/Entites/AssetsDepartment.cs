using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLearning.Domain.Entites
{
    public class AssetsDepartment
    {
        public Guid Id { get; set; }

        public string Code { get;set; }
        public string Name { get; set; }

        public ICollection<Assests> Assests { get; set; }

    }
}
