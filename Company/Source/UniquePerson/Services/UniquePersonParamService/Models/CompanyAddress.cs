using Sistran.Company.Application.ModelServices.Models.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonParamService.Models
{
    public class CompanyAddress
    {

        public int AddressTypeCd { get; set; }
        public string SmallDescription { get; set; }
        public string TinyDescription { get; set; }
        public bool IsElectronicMail{ get; set; }
    }
}
