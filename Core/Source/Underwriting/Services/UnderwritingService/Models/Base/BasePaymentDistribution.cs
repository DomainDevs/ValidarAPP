using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BasePaymentDistribution : Extension
    {
        /// <summary>
        /// Número
        /// </summary> 
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Cantidad De Calculo De Pago
        /// </summary> 
        [DataMember]
        public int? CalculationQuantity { get; set; }

        /// <summary>
        /// Porcentaje De Las Cuotas
        /// </summary> 
        [DataMember]
        public decimal Percentage { get; set; }
    }
}
