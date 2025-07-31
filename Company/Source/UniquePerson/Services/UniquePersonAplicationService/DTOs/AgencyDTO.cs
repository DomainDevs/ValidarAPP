using Sistran.Company.Application.CommonAplicationServices.Enums;
using Sistran.Company.Application.ModelServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class AgencyDTO 
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int AgentId { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public int AgentDeclinedTypeId { get; set; }

        [DataMember]
        public DateTime? DateDeclined { get; set; }

        [DataMember]
        public string Annotations { get; set; }
        [DataMember]
        public int AgenTypeId { get; set; }
        [DataMember]
        public string DescriptionBranch { get; set; }

 
        [DataMember]
        public int IndividualId { get; set; }
    }
}
