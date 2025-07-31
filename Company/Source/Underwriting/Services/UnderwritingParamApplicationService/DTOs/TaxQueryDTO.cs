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
    public class TaxQueryDTO
    {
        [DataMember]
        public List<TaxDTO> TaxDTOlist { get; set; }

        [DataMember]
        public ErrorDTO errorDTO { get; set; }
    }
}
