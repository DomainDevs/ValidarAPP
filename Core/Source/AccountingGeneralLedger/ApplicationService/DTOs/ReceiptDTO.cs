#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
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