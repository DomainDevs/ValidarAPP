using Sistran.Core.Application.CommonService.Models;
using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Cuotas de Pago
    /// </summary>
    [DataContract]
    public class BaseQuota : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Número
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Fecha de expiración
        /// </summary>
        [DataMember]
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Porcentaje
        /// </summary>
        [DataMember]
        public decimal Percentage { get; set; }
    }
}
