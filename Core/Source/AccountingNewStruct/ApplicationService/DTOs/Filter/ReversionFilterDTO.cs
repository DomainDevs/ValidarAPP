using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class ReversionFilterDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// AppId 
        /// </summary>        
        [DataMember]
        public int AppId { get; set; }
    }
}
