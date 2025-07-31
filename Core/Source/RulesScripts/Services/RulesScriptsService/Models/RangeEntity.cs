using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    using ModelServices.Models.Param;

    [DataContract]
    public class RangeEntity: ParametricServiceModel
    {
        /// <summary>
        /// Codigo del Rango
        /// </summary>
        [DataMember]
        public int RangeEntityCode { get; set; }

        /// <summary>
        /// Description de entidades de rango
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Id de Entidades de rango
        /// </summary>
        [DataMember]
        public int RangeValueAt { get; set; }

        /// <summary>
        /// Id de Entidades de rango
        /// </summary>
        [DataMember]
        public List<RangeEntityValue> RangeEntityValue { get; set; }
    }
}
