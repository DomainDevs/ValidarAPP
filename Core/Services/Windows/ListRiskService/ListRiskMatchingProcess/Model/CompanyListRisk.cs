using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class CompanyListRisk
    {
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Alias { get; set; }
        public int ListRiskId { get; set; }
        public string ListRiskDescription { get; set; }
        public int ListRiskType { get; set; }
        public string ListRiskTypeDescription { get; set; }
        public string CreatedUser { get; set; }
        public int ProcessId { get; set; }
        public int Event { get; set; }
    }
}
