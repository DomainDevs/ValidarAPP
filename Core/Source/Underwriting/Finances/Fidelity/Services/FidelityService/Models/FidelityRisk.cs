using Sistran.Core.Application.Finances.FidelityServices.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Finances.FidelityServices.Models
{
    /// <summary>
    /// Riesgo Manejo
    /// </summary>
    [DataContract]
    public class FidelityRisk : BaseFidelityRisk
    {
        [DataMember]
        public Risk Risk { get; set; }
    }
}
