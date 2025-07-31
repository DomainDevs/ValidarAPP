using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class ExchangeRateTolerance
    {
        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Porcentaje
        /// </summary>
        [DataMember]
        public decimal Percentage{ get; set; }

        /// <summary>
        /// Fecha incial
        /// </summary>
        [DataMember]
        public DateTime IntialDate { get; set; }

        /// <summary>
        /// Fecha final
        /// </summary>
        [DataMember]
        public DateTime EndDate { get; set; }
    }
}
