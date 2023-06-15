using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLearning.Contract.Dtos.Assets
{
    public class HistoryCheckBox
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsChecked { get; set; }
    }
}
