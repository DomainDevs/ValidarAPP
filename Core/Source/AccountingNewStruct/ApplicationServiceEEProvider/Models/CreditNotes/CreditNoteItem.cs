using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.CreditNotes
{
    /// <summary>
    /// CreditNoteItem:   Nota de Credito 
    /// </summary>
    [DataContract]
    public class CreditNoteItem
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
        public  Policy NegativePolicy { get; set; }

        /// <summary>
        /// PositivePolicy: Poliza - Endosos  Positivos 
        /// </summary>        
        [DataMember]
        public Policy PositivePolicy { get; set; }
       
    }
}
