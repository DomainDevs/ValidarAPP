using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyPaymentAccount : BasePaymentAccount
    {

        /// <summary>
        /// Banco sucursal 
        /// </summary>
        [DataMember]
        public CompanyBankBranch BankBranch { get; set; }

        /// <summary>
        /// Currency 
        /// </summary>
        [DataMember]
        public CompanyCurrency Currency { get; set; }

        /// <summary>
        /// Type 
        /// </summary>
        [DataMember]
        public CompanyPaymentAccountType Type { get; set; }

    }
}
