using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class ProspectLigthQuotationDTO
    {
        /// <summary>
        /// Código
        /// </summary>
        [DataMember]
        public int ProspectCode { get; set; }

        /// <summary>
        /// Nombres
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Primer apellido
        /// </summary>
        [DataMember]
        public string SurName { get; set; }

        /// <summary>
        /// Segundo apellido
        /// </summary>
        [DataMember]
        public string MotherLastName { get; set; }

        /// <summary>
        /// Identificador del género
        /// </summary>
        [DataMember]
        public string Gender { get; set; }

        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        [DataMember]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Dirección de residencia
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Número de teléfono
        /// </summary>
        [DataMember]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Dirección de correo electrónico
        /// </summary>
        [DataMember]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Tipo de prospecto
        /// </summary>
        [DataMember]
        public int IndividualTypePerson { get; set; }

        /// <summary>
        /// Identificador de tipo de documento
        /// </summary>
        [DataMember]
        public int CardId { get; set; }

        /// <summary>
        /// Número de documento
        /// </summary>
        [DataMember]
        public string CardDescription { get; set; }
    }
}
