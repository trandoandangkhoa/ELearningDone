using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLearning.Contract.Dtos.Assets.Moved
{
    public class AssetMovedHistoryDto
    {
        public string Id { get;set; }

        public DateTime DateCreated { get; set; }

        public ICollection<AssetsMovedDto> AssetMoveds { get; set; }
    }
}
