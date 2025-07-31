using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.CreditNotes
{
    /// <summary>
    /// CreditNote:   Nota de Crédito Proceso
    /// </summary>
    [DataContract]
    public class CreditNoteDTO
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
        public AmountDTO PositiveAppliedTotal  { get; set; }

        /// <summary>
        /// NegativeAppliedTotal: Total Negativo Aplicado 
        /// </summary>
        [DataMember]
        public AmountDTO NegativeAppliedTotal { get; set; }

        /// <summary>
        /// CreditNoteStatus: Estado de la Nota de Crédito
        /// </summary>        
        [DataMember]
        public int CreditNoteStatus { get; set; }

        /// <summary>
        /// CreditNoteItems: Lista de notas de credito
        /// </summary>        
        [DataMember]
        public List<CreditNoteItemDTO> CreditNoteItems { get; set; }
        
    }
}
