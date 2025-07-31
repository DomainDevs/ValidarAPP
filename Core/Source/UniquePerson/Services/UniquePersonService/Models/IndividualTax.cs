using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{

    /// <summary>
    /// Direcciones
    /// </summary>
    [DataContract]
    public class IndividualTax : BaseIndividualTax
    {
        /// <summary>
        /// Id de impuesto
        /// </summary>
        [DataMember]
        public Tax Tax { get; set; }

        /// <summary>
        /// Id de condicion de impuesto
        /// </summary>
        [DataMember]
        public TaxCondition TaxCondition { get; set; }

    }
}
