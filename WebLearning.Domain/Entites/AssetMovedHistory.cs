using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLearning.Domain.Entites
{
    public class AssetMovedHistory
    {
        public string Id { get;set; }

        public DateTime DateCreated { get; set; }

        public ICollection<AssetMoved> AssetMoveds { get; set; }
    }
}
