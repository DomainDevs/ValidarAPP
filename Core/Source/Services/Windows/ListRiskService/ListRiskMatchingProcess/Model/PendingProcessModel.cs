using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class PendingProcessModel
    {
        public int Id { get; set; }
        public string SearchValue { get; set; }
        public bool IsMasive { get; set; }
        public string listType { get; set; }
    }
}
