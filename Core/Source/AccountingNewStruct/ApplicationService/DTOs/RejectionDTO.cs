using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// Rejection: Motivo de Rechazo
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class RejectionDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description: Descripción del Motivo de Rechazo
        /// </summary>        
        [DataMember]
        public string Description { get; set; }
    }
}
