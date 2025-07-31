using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseIndividual : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

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
    }
}
