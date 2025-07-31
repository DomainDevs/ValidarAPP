using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables
{
    [DataContract]
    public class PaymentRequestNumberDTO
    {
        /// <summary>
        /// Id 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Branches 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public int BranchId { get; set; }
    }
}
