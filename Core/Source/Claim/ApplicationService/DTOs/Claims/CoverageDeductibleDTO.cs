using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class CoverageDeductibleDTO
    {
        [DataMember]
        public int Code { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
