using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CompanyIndividual
    {
        /// <summary>
        /// Gets or sets the identification document.
        /// </summary>
        /// <value>
        /// The identification document.
        /// </value>
        [DataMember]
        public IdentificationDocument IdentificationDocument { get; set; }
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Tipo de individuo
        /// </summary>
        [DataMember]
        public IndividualType IndividualType { get; set; }

        /// <summary>
        /// Nombre o Razón Social
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Primer Apellido
        /// </summary>
        [DataMember]
        public string Surname { get; set; }

        /// <summary>
        /// Segundo Apellido
        /// </summary>
        [DataMember]
        public string SecondSurname { get; set; }

        [DataMember]
        public EconomicActivity EconomicActivity { get; set; }

        [DataMember]
        public int? OwnerRoleCode { get; set; }

        /// <summary>
        /// Tipo de cliente
        /// </summary>
        [DataMember]
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// Tipo de cliente
        /// </summary>
        [DataMember]
        public string CustomerTypeDescription { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de Nacimiento o Creacion(Persona Juridica)
        /// </summary>
        [DataMember]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Medio de pago
        /// </summary>
        [DataMember]
        public IndividualPaymentMethod PaymentMethod { get; set; }
    }
}
