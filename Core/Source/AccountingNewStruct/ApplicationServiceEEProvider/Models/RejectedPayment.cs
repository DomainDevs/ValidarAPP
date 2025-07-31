using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

//Sistran
using PaymentsModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models
{
    /// <summary>
    /// RejectedPayment: Campos del Tipo de Item
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class RejectedPayment
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
        public PaymentsModels.Payment Payment { get; set; }

         /// <summary>
        /// Rejection: Motivo de Rechazo
        /// </summary>        
        [DataMember]
        public Rejection Rejection { get; set; }

        /// <summary>
        /// Date: Fecha de Rechazo
        /// </summary>        
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// Commission: Comision 
        /// </summary>        
        [DataMember]
        public Amount Commission { get; set; }

        /// <summary>
        /// TaxCommission: Impuesto de la Comision 
        /// </summary>        
        [DataMember]
        public Amount TaxCommission { get; set; }

        /// <summary>
        /// PaymentsTotal: Total de todos los pagos
        /// </summary>        
        [DataMember]
        public Amount PaymentsTotal { get; set; }

        /// <summary>
        /// Description: Descripción 
        /// </summary>        
        [DataMember]
        public string Description { get; set; }
    }
}
