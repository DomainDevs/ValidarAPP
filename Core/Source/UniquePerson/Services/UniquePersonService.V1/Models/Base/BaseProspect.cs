using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseProspect : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Razón Social
        /// </summary>
        [DataMember]
        public string TradeName { get; set; }

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
        /// Nombres
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Genero
        /// </summary>
        [DataMember]
        public string Gender { get; set; }

        /// <summary>
        /// Fecha de Nacimiento
        /// </summary>
        [DataMember]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Estado Civil
        /// </summary>
        [DataMember]
        public int MaritalStatus { get; set; }

        /// <summary>
        /// Prospecto de cotizador liviano
        /// </summary>
        [DataMember]
        public bool IsLightQuotation { get; set; }
    }
}
