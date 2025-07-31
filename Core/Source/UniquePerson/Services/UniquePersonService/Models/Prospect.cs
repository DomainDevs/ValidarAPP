using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Prospecto
    /// </summary>
    [DataContract]
    public class Prospect : BaseProspect
    {
        /// <summary>
        /// Tipo de Individuo
        /// </summary>
        [DataMember]
        public IndividualType IndividualType { get; set; }

        /// <summary>
        /// Tipo de Cliente
        /// </summary>
        [DataMember]
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// Dirección de Notificación
        /// </summary>
        [DataMember]
        public CompanyName CompanyName { get; set; }

        /// <summary>
        /// Identificación
        /// </summary>
        [DataMember]
        public IdentificationDocument IdentificationDocument { get; set; }
    }
}
