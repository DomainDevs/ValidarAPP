using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota
{
    [DataContract]
    public class Third
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public RiskCenter RiskCenter{ get; set; }
        [DataMember]
        public Restrictive Restrictive{ get; set; }
        [DataMember]
        public PromissoryNoteSignature PromissoryNoteSignature{ get; set; }
        [DataMember]
        public ReportListSisconc ReportListSisconc { get; set; }
        [DataMember]
        public DateTime CifinQuery { get; set; }
        [DataMember]
        public decimal PrincipalDebtor { get; set; }
        [DataMember]
        public decimal Cosigner { get; set; }
        [DataMember]
        public decimal Total { get; set; }

        [DataMember]
        public virtual List<PoliciesAut> InfringementPolicies { get; set; }

    }
}
