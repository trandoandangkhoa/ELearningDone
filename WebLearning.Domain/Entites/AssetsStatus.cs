using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLearning.Domain.Entites
{
    public  class AssetsStatus
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Assests> Assests { get; set; }
    }
}
