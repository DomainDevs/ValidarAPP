using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Sureties.SuretyServices.Models.Base;
using Sistran.Company.Application.Sureties.Models;

namespace Sistran.Company.Application.Sureties.SuretyServices.Models
{
    /// <summary>
    /// Cumplimiento - Riesgo
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.Sureties.SuretyServices.Models.Base.BaseContract" />
    [DataContract]
    public class CompanyContract : BaseContract
    {
        //[DataMember]
        //public new CompanyIssuanceInsured MainInsured { get; set; }
        // [DataMember]
        //   public string SettledNumber { get; set; }
        [DataMember]
        public CompanyContractClass Class { get; set; }
        //  [DataMember]
        //  public decimal Aggregate { get; set; }
        [DataMember]
        public OperatingQuota OperatingQuota { get; set; }
        //   [DataMember]
        //   public decimal Available { get; set; }
        [DataMember]
        public Amount Value { get; set; }
        [DataMember]
        public CompanyContractType ContractType { get; set; }
        [DataMember]
        public CompanyContractor Contractor { get; set; }
        [DataMember]
        public List<CiaRiskSuretyGuarantee> Guarantees { get; set; }
        [DataMember]
        public bool Isfacultative { get; set; }
        [DataMember]
        public CompanyText ContractObject { get; set; }
        [DataMember]
        public CompanyRisk Risk { get; set; }

        [DataMember]
        public CompanyRiskSuretyPost RiskSuretyPost { get; set; }
        [DataMember]
        public bool IsRetention { get; set; }

        [DataMember]
        public Country Country { get; set; }

        [DataMember]
        public State State { get; set; }

        [DataMember]
        public City City { get; set; }
    }
}