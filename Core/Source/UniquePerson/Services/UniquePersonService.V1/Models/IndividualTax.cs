using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.TaxServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{

    /// <summary>
    /// Direcciones
    /// </summary>
    [DataContract]
    public class IndividualTax : BaseIndividualTax
    {
        /// <summary>
        /// Tax
        /// </summary>
        [DataMember]
        public Tax Tax { get; set; }

        /// <summary>
        /// TaxRate
        /// </summary>
        [DataMember]
        
        public TaxRate TaxRate { get; set; }

        /// <summary>
        /// IndividualTaxExeption
        /// </summary>
        [DataMember]
        public IndividualTaxExeption IndividualTaxExeption { get; set; }
        /// <summary>
        /// Role
        /// </summary>
        [DataMember]
        public Role Role { get; set; }
        

    }
}
