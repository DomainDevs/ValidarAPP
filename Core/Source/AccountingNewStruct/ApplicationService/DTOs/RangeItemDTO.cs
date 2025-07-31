using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs
{
    /// <summary>
    /// RangeItem
    /// </summary>
    
    [DataContract]
    public class RangeItemDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Order
        /// </summary>
        [DataMember]
        public int Order { get; set; }
        
        /// <summary>
        /// RangeFrom: Rango Desde
        /// </summary>        
        [DataMember]
        public int RangeFrom { get; set; }

        /// <summary>
        /// RangeTo: Rango Hasta
        /// </summary>        
        [DataMember]
        public int RangeTo { get; set; }

    }
}
