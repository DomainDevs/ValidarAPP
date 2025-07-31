

using System.Runtime.Serialization;

namespace Sistran.Company.Application.Transports.TransportApplicationService.DTOs
{
    [DataContract]
    public class DeductibleUnitDTO
    {
        /// <summary>
        /// Identificador del DeductibleUnit
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción del DeductibleUnit
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool HasSubject { get; set; }
    }
}
