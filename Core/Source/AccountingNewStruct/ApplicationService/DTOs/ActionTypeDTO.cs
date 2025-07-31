using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// ActionType: 
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class ActionTypeDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description: Descripción de la Acción
        /// </summary>        
        [DataMember]
        public string Description { get; set; }
    }
}
