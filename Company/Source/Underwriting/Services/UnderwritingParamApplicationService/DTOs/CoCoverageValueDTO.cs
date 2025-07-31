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
    public class CoCoverageValueDTO
    {        
        [DataMember]
        public decimal? Porcentage { get; set;}

        [DataMember]
        public PrefixDTO Prefix { get; set;}

        [DataMember]
        public CoverageDTO Coverage { get; set;}

        [DataMember]
        public ErrorDTO Error { get; set;}
    }
   
}
