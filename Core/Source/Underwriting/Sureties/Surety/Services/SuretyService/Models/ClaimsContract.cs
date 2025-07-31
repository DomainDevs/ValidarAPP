using Sistran.Core.Application.Sureties.SuretyServices.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
namespace Sistran.Core.Application.Sureties.SuretyServices.Models
{
    [DataContract]
    public class ClaimsContract : BaseContract
    {
        [DataMember]
        public BaseContractClass Class { get; set; }

        [DataMember]
        public OperatingQuota OperatingQuota { get; set; }
   
        [DataMember]
        public Amount Value { get; set; }
        [DataMember]
        public BaseContractType ContractType { get; set; }
        [DataMember]
        public CompanyContractor Contractor { get; set; }
        [DataMember]
        public List<CompanyGuarantee> Guarantees { get; set; }
        [DataMember]
        public bool Isfacultative { get; set; }
        [DataMember]
        public UnderwritingServices.Models.Base.BaseText ContractObject { get; set; }
        [DataMember]
        public CompanyRisk Risk { get; set; }

        [DataMember]
        public CompanyRiskSuretyPost RiskSuretyPost { get; set; }
        [DataMember]
        public bool IsRetention { get; set; }
    }
}
