using Sistran.Company.Application.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    [DataContract]
    public class TaxConditionDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IdTax { get; set; }

        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool HasNationalRate { get; set; }
        [DataMember]
        public bool IsIndependent { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }

        [DataMember]
        public ErrorDTO errorDTO { get; set; }
    }
}