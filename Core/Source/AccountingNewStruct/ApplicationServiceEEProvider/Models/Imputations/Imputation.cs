using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

//Sistran
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.Enums;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// Imputation:   imputación
    /// </summary>
    [DataContract]
    public class Imputation
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
        public ImputationTypes ImputationType { get; set; }

        /// <summary>
        /// IsTemporal: Se trata de un recibo temporal?
        /// </summary>        
        [DataMember]
        public bool IsTemporal { get; set; }

        /// <summary>
        /// ImputationItems: Items de la imputación 
        /// </summary>        
        [DataMember]
        public List<TransactionType> ImputationItems { get; set; }

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
        public Amount VerificationValue { get; set; }

      
    }
}
