using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.CreditNotes
{
    /// <summary>
    /// CreditNoteItem:   Nota de Credito 
    /// </summary>
    [DataContract]
    public class CreditNoteItemDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// IsApplied: Es considerado en el Proceso 
        /// </summary>        
        [DataMember]
        public bool IsApplied { get; set; }

        /// <summary>
        /// NegativePolicy: Poliza - Endoso  Negativo
        /// </summary>        
        [DataMember]
        public  PolicyDTO NegativePolicy { get; set; }

        /// <summary>
        /// PositivePolicy: Poliza - Endosos  Positivos 
        /// </summary>        
        [DataMember]
        public PolicyDTO PositivePolicy { get; set; }      
    }
}
