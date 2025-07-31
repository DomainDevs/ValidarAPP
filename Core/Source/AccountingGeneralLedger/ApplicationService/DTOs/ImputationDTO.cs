using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class ImputationDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        ///// <summary>
        ///// ImputationType: Tipo de Imputacion 
        ///// </summary>        
        [DataMember]
        public int ImputationType { get; set; }

        /// <summary>
        /// IsTemporal: Se trata de un recibo temporal?
        /// </summary>        
        [DataMember]
        public bool IsTemporal { get; set; }

        /// <summary>
        /// ImputationItems: Items de la imputación 
        /// </summary>        
        [DataMember]
        public List<TransactionTypeDTO> ImputationItems { get; set; }

        /// <summary>
        /// User 
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Date 
        /// </summary>        
        [DataMember]
        public DateTime Date { get; set; }
        /// <summary>
        /// VerificationValue : Diferencia de Control
        /// </summary>        
        [DataMember]
        public AmountDTO VerificationValue { get; set; }
    }
}
