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
    /// Administrador de listas
    /// </summary>
    /// <author>Germán F. Grimaldi</author>
    /// <date>16/08/2018</date>
    [DataContract]
    public class AllyCoverageQueryDTO
    {
        [DataMember]
        public List<AllyCoverageDTO> AllyCoverageDTO { get; set; }

        [DataMember]
        public ErrorDTO ErrorDTO { get; set; }
    }
}
