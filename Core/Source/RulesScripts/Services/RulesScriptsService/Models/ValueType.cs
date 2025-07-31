using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class ValueType
    {
        /// <summary>
        /// Codigo
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}



