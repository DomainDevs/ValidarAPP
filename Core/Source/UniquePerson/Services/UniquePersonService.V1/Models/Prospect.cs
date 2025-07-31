using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
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
