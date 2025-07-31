using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{   
    [DataContract]
    public class ReinsurancePaymentClaimLayerDTO
    {
        /// <summary>
        /// Id
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Description
        /// </summary>        
        [DataMember]
        public string Description { get; set; }
    }
}