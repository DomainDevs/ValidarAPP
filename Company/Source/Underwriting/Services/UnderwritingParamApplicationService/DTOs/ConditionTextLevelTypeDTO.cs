using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamApplicationService.DTOs
{
    /// <summary>
    /// Modelo DTO ConditionTextLevelTypeDTO
    /// </summary>
    [DataContract]
    public class ConditionTextLevelTypeDTO
    {
        /// <summary>
        /// Get or sets Id.
        /// </summary>
        [DataMember]
        public int Id { get; set; }
        /// <summary>
        /// Get or sets Description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
