using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Prospectos
    /// </summary>
    [DataContract]
    public class ProspectNatural : BaseProspectNatural
    {
        [DataMember]
        public City City { get; set; }
        [DataMember]
        public string AdditionalInfo { get; set; }
    }
}
