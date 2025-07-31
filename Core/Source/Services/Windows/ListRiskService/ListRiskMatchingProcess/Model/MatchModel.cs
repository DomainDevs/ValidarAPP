using ListRiskMatchingProcess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class MatchModel
    {
        public int? RequestId { get; set; }

        public int ProcessId { get; set; }

        public string DocumentType { get; set; }

        public string DocumentNumber { get; set; }

        public string Names { get; set; }

        public string LastNames { get; set; }

        public string Movement { get; set; }

        public string Role { get; set; }

        public string MovementId { get; set; }

        public DateTime MovementDate { get; set; }

        public DateTime MovementFrom { get; set; }

        public DateTime MovementTo { get; set; }

        public bool ONUMatch { get; set; }

        public bool OFACMatch { get; set; }

        public bool OWNMatch { get; set; }

        public int StatusId { get; set; }

        public string ListDescription { get; set; }
    }
}
