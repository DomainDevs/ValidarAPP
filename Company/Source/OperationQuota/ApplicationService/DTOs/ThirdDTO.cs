using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.DTOs
{
    [DataContract]
    public class ThirdDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public RiskCenterDTO RiskCenterDTO{ get; set; }
        [DataMember]
        public RestrictiveDTO RestrictiveDTO{ get; set; }
        [DataMember]
        public PromissoryNoteSignatureDTO PromissoryNoteSignatureDTO{ get; set; }
        [DataMember]
        public ReportListSisconcDTO ReportListSisconcDTO { get; set; }
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
