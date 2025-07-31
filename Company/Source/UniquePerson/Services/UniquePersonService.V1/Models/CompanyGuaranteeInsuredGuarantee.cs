using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyGuaranteeInsuredGuarantee : BaseGeneric
    {

        /// <summary>
        /// IndividualId
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Tipo contragarantia
        /// </summary>
        [DataMember]
        public int typeId { get; set; }

    }
}
