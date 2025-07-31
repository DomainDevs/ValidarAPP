using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// RejectedPayment: Campos del Tipo de Item
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class RejectedPaymentDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Payment: Pago efectuado para el rechazo
        /// </summary>        
        [DataMember]
        public PaymentDTO Payment { get; set; }

        /// <summary>
        /// Rejection: Motivo de Rechazo
        /// </summary>        
        [DataMember]
        public RejectionDTO Rejection { get; set; }

        /// <summary>
        /// Date: Fecha de Rechazo
        /// </summary>        
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Commission: Comision 
        /// </summary>        
        [DataMember]
        public AmountDTO Commission { get; set; }

        /// <summary>
        /// TaxCommission: Impuesto de la Comision 
        /// </summary>        
        [DataMember]
        public AmountDTO TaxCommission { get; set; }

        /// <summary>
        /// PaymentsTotal: Total de todos los pagos
        /// </summary>        
        [DataMember]
        public AmountDTO PaymentsTotal { get; set; }

        /// <summary>
        /// Description: Descripción 
        /// </summary>        
        [DataMember]
        public string Description { get; set; }
    }
}
