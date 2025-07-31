using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    [DataContract]
    public class EndorsementDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public List<RiskDTO> Risks { get; set; }
        [DataMember]
        public int RiskId { get; set; }

        [DataMember]
        public decimal Prime { get; set; }
        [DataMember]
        public string OperationType { get; set; }
        [DataMember]
        public string InsuredName { get; set; }
        [DataMember]
        public int InsuredCd { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int EndorsementNumber { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public DateTime CurrentTo { get; set; }
        [DataMember]
        public DateTime CurrentFrom { get; set; }
        [DataMember]
        public DateTime IssueDate { get; set; }
        [DataMember]
        public decimal InsuredAmount { get; set; }
        [DataMember]
        public decimal ResponsibilityMaximumAmount { get; set; }
    }
}
