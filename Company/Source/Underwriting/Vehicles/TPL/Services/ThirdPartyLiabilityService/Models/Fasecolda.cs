using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models
{
    /// <summary>
    /// Fasecolda
    /// </summary>
    [DataContract]
    public class Fasecolda
    {
        /// <summary>
        /// Codigo Fasecolda 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Modelo
        /// </summary>
        [DataMember]
        public string Model { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        [DataMember]
        public string Make { get; set; }
    }
}
