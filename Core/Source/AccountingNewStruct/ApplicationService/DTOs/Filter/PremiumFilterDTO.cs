using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Filter
{
    [DataContract]
    public class PremiumFilterDTO
    {
        /// <summary>
        /// Id Endoso
        /// </summary>        
        [DataMember]
        public int Id { get; set; }     
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public int PremiumId { get; set; }
        [DataMember]
        public bool IsReversion { get; set; }


    }
}
