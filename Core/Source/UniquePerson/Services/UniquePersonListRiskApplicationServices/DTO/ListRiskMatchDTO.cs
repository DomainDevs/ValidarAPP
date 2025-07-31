using Sistran.Core.Application.UniquePersonListRiskApplicationServices.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO
{
    public class ListRiskMatchDTO
    {
        public string jModel { get; set; }
        public int listVersion { get; set; }
        public string listType { get; set; }
        public DateTime registrationDate { get; set; }
        public string fileName { get; set; }
        public ProcessStatusEnum Status { get; set; }
    }
}
