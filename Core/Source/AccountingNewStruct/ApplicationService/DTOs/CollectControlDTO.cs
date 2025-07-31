using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;



namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class CollectControlDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// User: Usuario
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Branch: Sucursal del pago
        /// </summary>        
        [DataMember]
        public SCRDTO.BranchDTO Branch { get; set; }

        /// <summary>
        /// State: Estado 1 abierto , 0 cerrado
        /// </summary>        
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// Date: Fecha contable
        /// </summary>        
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Date: Fecha de apertura
        /// </summary>        
        [DataMember]
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// Date: Fecha de apertura
        /// </summary>        
        [DataMember]
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Collects:Ingresos 
        /// </summary>        
        [DataMember]
        public List<CollectDTO> Collects { get; set; }

        /// <summary>
        /// CollectControlPayments: Control de Pagos
        /// </summary>        
        [DataMember]
        public List<CollectControlPaymentDTO> CollectControlPayments { get; set; }
    }
}
