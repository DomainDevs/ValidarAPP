
using SCRDTO = Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{

    /// <summary>
    /// JournalEntry: Asiento Diario
    /// </summary>
    [DataContract]
    public class JournalEntryDTO
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
        public SCRDTO.BranchDTO Branch { get; set; }

         /// <summary>
        /// SalePoint: Punto de Venta
        /// </summary> 
        [DataMember]
        public SalePointDTO SalePoint { get; set; }

         /// <summary>
        /// Company: Compañia
        /// </summary> 
        [DataMember]
        public CompanyDTO Company { get; set; }

         /// <summary>
        /// Payer: Pagador, Abonador
        /// </summary> 
        [DataMember]
        public IndividualDTO Payer { get; set; }

        /// <summary>
        /// PersonType: Tipo de Pagador
        /// </summary> 
        [DataMember]
        public PersonTypeDTO PersonType { get; set; }
        
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
        public ApplicationDTO Imputation { get; set; }

        /// <summary>
        /// Transaction: Transaccion 
        /// </summary> 
        [DataMember]
        public TransactionDTO Transaction { get; set; }

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
