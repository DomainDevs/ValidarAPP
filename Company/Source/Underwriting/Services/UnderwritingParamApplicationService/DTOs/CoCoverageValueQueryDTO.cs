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
     public class CoCoverageValueQueryDTO
    {
        [DataMember]
        public List<CoCoverageValueDTO> CoCoverageValue { get; set;}

        [DataMember]
        public ErrorDTO Error { get; set;}
    }
}
