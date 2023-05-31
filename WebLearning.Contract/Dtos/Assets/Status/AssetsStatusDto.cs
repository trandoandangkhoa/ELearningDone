using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLearning.Contract.Dtos.Assets.Status
{
    public class AssetsStatusDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<AssetsDto> AssetsDtos { get; set; }
    }
}
