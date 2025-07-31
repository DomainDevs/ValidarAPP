using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CoConsurtuimCompany: BaseCompany
    {
        [DataMember]
        /// <summary>
        /// role
        /// </summary>
        public CompanyRole Role { get; set; }

        /// <summary>
        /// EconomicActivity
        /// </summary>
        public CompanyEconomicActivity EconomicActivity { get; set; }

        /// <summary>
        /// IdentificationDocument
        /// </summary>
        public CompanyIdentificationDocument IdentificationDocument { get; set; }

        /// <summary>
        /// CompanyType
        /// </summary>
        public CompanyCompanyType CompanyType { get; set; }

        /// <summary>
        /// AssociationType
        /// </summary>
        public CompanyAssociationType AssociationType { get; set; }
    }
}
