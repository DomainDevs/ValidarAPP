using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServices.Models
{
    [DataContract]
    public class CompanyCustomerKnowledge
    {
        [DataMember]
        public CompanyFinancialSarlaft FinancialSarlaft { get; set; }

        [DataMember]
        public CompanyIndividualSarlaft Sarlaft { get; set; }
       

    }
}
