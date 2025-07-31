using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ExternalPrinterServices.Models
{
    public class GenerateWSTempPrinterRequest
    {
        public int TempId { get; set; }
        public int BranchCd { get; set; }
        public int PrefixNum { get; set; }
        public long DocumentNumber { get; set; }
        public string EmailUser { get; set; }
        public bool PrintBinary { get; set; }

    }
}
