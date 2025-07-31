using Sistran.Core.Application.Extensions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base
{
    [DataContract]
    public class BaseIssuanceInsured : Extension
    {
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int InsuredId { get; set; }

        [DataMember]
        public int? OwnerRoleCode { get; set; }

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
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        [DataMember]
        public string Gender { get; set; }

        /// <summary>
        /// Tipo de individuo
        /// </summary>
        [DataMember]
        public IndividualType IndividualType { get; set; }
        
        /// <summary>
        /// Tipo de cliente
        /// </summary>
        [DataMember]
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// Gets or sets the declined date.
        /// </summary>
        /// <value>
        /// The declined date.
        /// </value>
        [DataMember]
        public DateTime? DeclinedDate { get; set; }
    }
}