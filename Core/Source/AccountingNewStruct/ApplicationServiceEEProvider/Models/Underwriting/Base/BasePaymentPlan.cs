using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base
{
    /// <summary>
    /// Planes de Pago
    /// </summary>
    [DataContract]
    public class BasePaymentPlan : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Predeterminado
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }
    }
}
