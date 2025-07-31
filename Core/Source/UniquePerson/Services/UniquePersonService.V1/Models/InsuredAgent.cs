using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    public class InsuredAgent: BaseInsuredAgent
    {
        [DataMember]
        public Insured InsuredIndId { get; set; }

        [DataMember]
        public Agent AgentIndId { get; set; }

        [DataMember]
        public Agency AgentAgencyId { get; set; }
    }
}
