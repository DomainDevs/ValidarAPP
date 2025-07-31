using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
//Sistran
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{

    /// <summary>
    /// JournalEntry: Asiento Diario
    /// </summary>
    [DataContract]
    public class JournalEntry
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        /// AccountingDate: Fecha Contable 
        /// </summary> 
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Branch: Sucursal
        /// </summary> 
        [DataMember]
        public Branch Branch { get; set; }

         /// <summary>
        /// SalePoint: Punto de Venta
        /// </summary> 
        [DataMember]
        public SalePoint SalePoint { get; set; }

         /// <summary>
        /// Company: Compañia
        /// </summary> 
        [DataMember]
        public Company Company { get; set; }

         /// <summary>
        /// Payer: Pagador, Abonador
        /// </summary> 
        [DataMember]
        public Individual Payer { get; set; }

        /// <summary>
        /// PersonType: Tipo de Pagador
        /// </summary> 
        [DataMember]
        public PersonType PersonType { get; set; }
        
        /// <summary>
        /// Description 
        /// </summary> 
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Description 
        /// </summary> 
        [DataMember]
        public string Comments { get; set; }

        /// <summary>
        /// Imputation: Imputacion 
        /// </summary> 
        [DataMember]
        public Application Imputation { get; set; }

        /// <summary>
        /// Transaction: Transaccion 
        /// </summary> 
        [DataMember]
        public Transaction Transaction { get; set; }

        /// <summary>
        /// Status: Estado 
        /// </summary> 
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// IsTemporal: Se trata de un transaccion temporal?
        /// </summary>        
        [DataMember]
        public bool IsTemporal { get; set; }

    }
}
