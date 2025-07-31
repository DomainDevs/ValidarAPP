using System.Runtime.Serialization;

namespace Sistran.Core.Application.RulesScriptsServices.Models
{
    [DataContract]
    public class BasicType
    {
        /// <summary>
        /// propiedad BasicTypeId
        /// </summary>
        [DataMember]
        public int BasicTypeId { get; set; }

        /// <summary>
        /// propiedad Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}