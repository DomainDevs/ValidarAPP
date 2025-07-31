using Sistran.Core.Application.CommonService.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    [DataContract]
    public class PaymentAccount
    {
        /// <summary>
        /// Banco sucursal 
        /// </summary>
        [DataMember]
        public BankBranch BankBranch { get; set; }

        /// <summary>
        /// Currency 
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        /// Type 
        /// </summary>
        [DataMember]
        public PaymentAccountType Type { get; set; }
    }
}
