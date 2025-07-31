using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs
{
    /// <summary>
    ///     Modelo que representa los Comprobantes
    /// </summary>
    [DataContract]
    public class ReceiptDTO
    {
        /// <summary>
        ///     Identificador único del modelo
        /// </summary>
        [DataMember]
        public int ReceiptId { get; set; }

        /// <summary>
        ///     Número de comprobante
        /// </summary>
        [DataMember]
        public int? Number { get; set; }

        /// <summary>
        ///     Fecha de comprobante
        /// </summary>
        [DataMember]
        public DateTime? Date { get; set; }
    }
}
