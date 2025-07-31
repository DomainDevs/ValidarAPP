using System.Runtime.Serialization;

namespace Sistran.Company.Application.Transports.TransportApplicationService.DTOs
{
    public class DeductibleSubjectDTO
    {
        /// <summary>
        /// Identificador del DeductibleSubject
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción del DeductibleSubjectDTO
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
