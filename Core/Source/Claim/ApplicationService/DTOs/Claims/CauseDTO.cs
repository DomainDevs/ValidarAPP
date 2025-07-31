using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class CauseDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public string PrefixDescription { get; set; }

        [DataMember]
        public bool IsPoliceComplaintRequired { get; set; }

        [DataMember]
        public string IsPoliceComplaintRequiredDescription { get; set; }

        [DataMember]
        public bool IsDriverInformationRequired { get; set; }

        [DataMember]
        public string IsDriverInformationRequiredDescription { get; set; }

        [DataMember]
        public bool IsInspectionDateRequired { get; set; }

        [DataMember]
        public string IsInspectionDateRequiredDescription { get; set; }
    }
}
