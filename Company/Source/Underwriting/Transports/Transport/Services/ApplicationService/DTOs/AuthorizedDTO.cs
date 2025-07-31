using System.Runtime.Serialization;

namespace Sistran.Company.Application.Transports.TransportApplicationService.DTOs
{
    [DataContract]
    public class AuthorizedDTO
    {
        /// <summary>
        /// Indica si la operación está autorizada
        /// </summary>
        [DataMember]
        public bool Authorized { get; set; }

        /// <summary>
        /// Mensaje
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}
