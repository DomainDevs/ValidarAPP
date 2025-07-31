using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class AmountDTO
    {
        /// <summary>
        /// Valor
        /// </summary>
        [DataMember]
        public decimal Value { get; set; }

        /// <summary>
        /// Información de moneda
        /// </summary>
        public CurrencyDTO Currency { get; set; }
    }
}
