using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseProvider : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// IndividualID Person
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Id Tipo de proveedor
        /// </summary>
        [DataMember]
        public int ProviderTypeId { get; set; }

        /// <summary>
        /// Id Tipo de origen
        /// </summary>
        [DataMember]
        public int OriginTypeId { get; set; }

        /// <summary>
        /// Id Tipo de baja
        /// </summary>
        [DataMember]
        public int? ProviderDeclinedTypeId { get; set; }

        /// <summary>
        /// Fecha de creación
        /// </summary>
        [DataMember]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Fecha de modificacion
        /// </summary>
        [DataMember]
        public DateTime? ModificationDate { get; set; }

        /// <summary>
        /// Fecha de baja
        /// </summary>
        [DataMember]
        public DateTime? DeclinationDate { get; set; }

        /// <summary>
        /// Observacion
        /// </summary>
        [DataMember]
        public string Observation { get; set; }

        /// <summary>
        /// Especialidad predeterminada
        /// </summary>
        [DataMember]
        public int? SpecialityDefault { get; set; }
    }
}
