using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    [DataContract]
    public class ReinsuranceCheckingAccountItemDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// TempReinsuranceCheckingAccountId 
        /// </summary>        
        [DataMember]
        public int TempReinsuranceCheckingAccountId { get; set; }


        /// <summary>
        /// ReinsuranceCheckingAccountId 
        /// </summary>        
        [DataMember]
        public int ReinsuranceCheckingAccountId { get; set; }
    }
}
