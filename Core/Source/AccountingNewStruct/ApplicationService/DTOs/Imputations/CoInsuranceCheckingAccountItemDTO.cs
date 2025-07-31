using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    [DataContract]
    public class CoInsuranceCheckingAccountItemDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// TempCoinsuranceCheckingAccountId 
        /// </summary>        
        [DataMember]
        public int TempCoinsuranceCheckingAccountId { get; set; }


        /// <summary>
        /// CoinsuranceCheckingAccountId 
        /// </summary>        
        [DataMember]
        public int CoinsuranceCheckingAccountId { get; set; }
    }
}
