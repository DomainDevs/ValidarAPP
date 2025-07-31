using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    using Core.Application.AuthorizationPoliciesServices.Models;
    using Core.Application.UniquePersonService.V1.Models;

    [DataContract]
    public class GuaranteeDto
    {
        [DataMember]
        public Guarantee Guarantee { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

        [DataMember]
        public int OperationId { get; set; }
    }
}
