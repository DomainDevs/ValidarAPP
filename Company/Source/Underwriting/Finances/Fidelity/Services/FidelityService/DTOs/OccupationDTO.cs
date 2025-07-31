using System.Runtime.Serialization;

namespace Sistran.Company.Application.Location.FidelityServices.DTOs
{
    [DataContract]
    public class OccupationDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
