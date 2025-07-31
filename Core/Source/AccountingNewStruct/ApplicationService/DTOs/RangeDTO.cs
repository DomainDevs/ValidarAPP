using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// Range
    /// </summary>
    
    [DataContract]
    public class RangeDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }
        
        /// <summary>
        /// Description: Descripcion
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// IsDefault
        /// </summary>
        [DataMember]
        public bool IsDefault { get; set; }

        /// <summary>
        /// RangeItems
        /// </summary>
        [DataMember]
        public List<RangeItemDTO> RangeItems { get; set; }

    }
}
