using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.TaxServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{

    /// <summary>
    /// Direcciones
    /// </summary>
    [DataContract]
    public class CompanyIndividualTax : BaseIndividualTax
    {
        [DataMember]
        public TaxRate  taxRate { get; set; }

        /// <summary>
        /// Id de condicion de impuesto
        /// </summary>
        [DataMember]
        public CompanyIndividualTaxExeption IndividualTaxExeption { get; set; }
        /// <summary>
        /// Individual Role
        /// </summary>
        [DataMember]
        public Role Role { get; set; }

    }
}
