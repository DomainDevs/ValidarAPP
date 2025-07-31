using System.Runtime.Serialization;

namespace Sistran.Company.Application.CommonServices.Models
{
    [DataContract]
    public class Nomenclature
    {
        /// <summary>
        /// Nomenclatura dirección
        /// </summary>
        [DataMember]
        public string Nomenclatura { get; set; }

        /// <summary>
        /// Abreviatura dirección
        /// </summary>
        [DataMember]
        public string Abreviatura { get; set; }
    }
}
