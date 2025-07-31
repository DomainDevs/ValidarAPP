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
    public class PrefixDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int AgentId { get; set; }

        [DataMember]
        public string Description { get; set; }
      

    }
}
