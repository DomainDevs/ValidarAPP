
using System.Data;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class DecisionTableMappingResult
    {
        /// <summary>
        /// propiedad RuleBase
        /// </summary>
        [DataMember]
        public RuleBase RuleBase { get; set; }

        /// <summary>
        /// propiedad CountActions
        /// </summary>
        [DataMember]
        public int CountActions { get; set; }

        /// <summary>
        /// propiedad CountCondition
        /// </summary>
        [DataMember]
        public int CountCondition { get; set; }

        /// <summary>
        /// propiedad CountRules
        /// </summary>
        [DataMember]
        public int CountRules { get; set; }

        /// <summary>
        /// propiedad DataSet
        /// </summary>
        [DataMember]
        public DataTable DataSet { get; set; }
    }
}
