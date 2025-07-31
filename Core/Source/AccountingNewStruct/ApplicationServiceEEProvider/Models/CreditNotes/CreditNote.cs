using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.CommonService.Models;


namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.CreditNotes
{
    /// <summary>
    /// CreditNote:   Nota de Crédito Proceso
    /// </summary>
    [DataContract]
    public class CreditNote
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Date: Fecha de proceso 
        /// </summary>        
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// User: Usuario 
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// PositiveAppliedTotal: Total Positivo Aplicado 
        /// </summary>
        [DataMember]
        public Amount PositiveAppliedTotal  { get; set; }

        /// <summary>
        /// NegativeAppliedTotal: Total Negativo Aplicado 
        /// </summary>
        [DataMember]
        public Amount NegativeAppliedTotal { get; set; }

        /// <summary>
        /// CreditNoteStatus: Estado de la Nota de Crédito
        /// </summary>        
        [DataMember]
        public CreditNoteStatus CreditNoteStatus { get; set; }

        /// <summary>
        /// CreditNoteItems: Lista de notas de credito
        /// </summary>        
        [DataMember]
        public List<CreditNoteItem> CreditNoteItems { get; set; }
        
    }
}
