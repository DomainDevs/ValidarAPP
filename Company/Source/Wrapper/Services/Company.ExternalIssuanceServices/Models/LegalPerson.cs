using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ExternalIssuanceServices.Models
{
    public class LegalPerson
    {
        public int TributaryTypeCode { get; set; }
        public string TributaryIdNo { get; set; }
        public string TradeName { get; set; }
        public int CompanyTypeCode { get; set; }
    }
}
