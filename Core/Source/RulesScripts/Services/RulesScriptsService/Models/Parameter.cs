using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class Parameter
    {
        /// <summary>
        /// Tipo
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Clase
        /// </summary>
        [DataMember]
        public string Class { get; set; }
    }
}
