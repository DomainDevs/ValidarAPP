using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{

    [DataContract]
    public class CompanyIndividualPaymentMethod : BaseIndividualPaymentMethod
    {

        /// <summary>
        /// PaymentMethodAccount
        /// </summary>
        [DataMember]
        public CompanyPaymentAccount Account { get; set; }

        /// <summary>
        /// PaymentMethod
        /// </summary>
        [DataMember]
        public CompanyPaymentMethod Method { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        [DataMember]
        public CompanyRole Rol { get; set; }

    }
}
