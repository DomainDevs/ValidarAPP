using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Dirección de Notificación
    /// </summary>
    [DataContract]
    public class CompanyName : BaseCompanyName
    {
        /// <summary>
        /// Direccion
        /// </summary>
        [DataMember]
        public Address Address { get; set; }

        /// <summary>
        /// Teléfono 
        /// </summary>
        [DataMember]
        public Phone Phone { get; set; }

        /// <summary>
        /// Correo 
        /// </summary>
        [DataMember]
        public Email Email { get; set; }
    }
}