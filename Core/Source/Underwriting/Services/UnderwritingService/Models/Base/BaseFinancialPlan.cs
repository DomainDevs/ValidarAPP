using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Planes de financiacion
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseFinancialPlan : Extension
    {
        /// <summary>
        /// Atributo para la propiedad Id
        /// </summary> 
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Atributo para la propiedad PaymentPlanId
        /// </summary> 
        [DataMember]
        public int PaymentPlanId { get; set; }

        /// <summary>
        /// Gets or sets valor por dfecti
        /// </summary>
        /// <value>
        /// IsDefault
        /// </value>
        [DataMember]
        public bool IsDefault { get; set; }
    }
}
