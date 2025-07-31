using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class CompanyListRiskOfac
    {
        public int EntNum { get; set; }
        public string SDNName { get; set; }
        public string SDNType { get; set; }
        public string Program { get; set; }
        public string Title { get; set; }
        public string CallSign { get; set; }
        public string VessType { get; set; }
        public string Tonnage { get; set; }
        public string GRT { get; set; }
        public string VessFlag { get; set; }
        public string VessOwner { get; set; }
        public string Remarks { get; set; }
        public int ListRiskType { get; set; }
        public int ProcessId { get; set; }
        public string CreatedUser { get; set; }
        public string ListRiskDescription { get; set; }
        public string ListRiskTypeDescription { get; set; }
        public int Event { get; set; }
    }
}
