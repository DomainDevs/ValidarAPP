using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyIssuanceCoInsuranceCompany: IssuanceCoInsuranceCompany
    {
        /// <summary>
        /// Beneficiario
        /// </summary>
        [DataMember]
        public List<CompanyAcceptCoInsuranceAgent> acceptCoInsuranceAgent { get; set; }
        
    }
}
