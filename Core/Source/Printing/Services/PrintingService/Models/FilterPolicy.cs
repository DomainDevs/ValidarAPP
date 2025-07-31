using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.PrintingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.PrintingServices.Models
{
    [DataContract]
    public class FilterPolicy : BaseFilterPolicy
    {
        /// <summary>
        /// Tipo de Riesgo
        /// </summary>
        [DataMember]
        public CoveredRiskType? CoveredRiskType { get; set; }
    }
}
