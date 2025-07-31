using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class ConditionLevel : BaseConditionLevel
    {

        /// <summary>
        /// Nivel
        /// </summary>
        [DataMember]
        public EmissionLevel EmissionLevel { get; set; }

        /// <summary>
        /// Paquete
        /// </summary>
        [DataMember]
        public Package Package { get; set; }
    }
}