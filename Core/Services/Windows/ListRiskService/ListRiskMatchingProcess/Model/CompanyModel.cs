using ListRiskMatchingProcess.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class CompanyModel
    {
        public int IndividualId { get; set; }
        public string TradeName { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
    }
}
