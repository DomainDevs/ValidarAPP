using Sistran.Company.Application.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    /// <summary>
    /// AllyCoverageDTO. Modelo de ciudad DTO.
    /// </summary>
    [DataContract]
    public class AllyCoverageDTO
    {
        [DataMember]
        public int AllyCoverageId { get; set; }
        [DataMember]
        public int CoverageId { get; set; }
        [DataMember]
        public decimal CoveragePct { get; set; }
        [DataMember]
        public ErrorDTO errorDTO { get; set; }
    }
}
