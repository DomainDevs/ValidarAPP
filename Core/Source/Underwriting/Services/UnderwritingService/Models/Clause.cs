using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Clausula
    /// </summary>
    [DataContract]
    public class Clause: BaseClause
    {
        /// <summary>
        /// Nivel De Condición
        /// </summary>
        [DataMember]
        public ConditionLevel ConditionLevel { get; set; }
    }
}