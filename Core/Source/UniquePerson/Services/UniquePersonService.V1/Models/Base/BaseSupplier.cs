using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseSupplier : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Fecha de baja
        /// </summary>
        [DataMember]
        public DateTime? EnteredDate { get; set; }

        /// <summary>
        /// Fecha de baja
        /// </summary>
        [DataMember]
        public DateTime? DeclinedDate { get; set; }

        /// <summary>
        /// Razon de baja
        /// </summary>
        [DataMember]
        public string DeclinedReason { get; set; }

        /// <summary>
        /// Activo
        /// </summary>
        [DataMember]
        public bool? Enabled { get; set; }

        /// <summary>
        /// Cheque pagadero a
        /// </summary>
        [DataMember]
        public string CheckPayableTo { get; set; }

        /// <summary>
        /// Verificación de orden
        /// </summary>
        [DataMember]
        public bool? OrderCheck { get; set; }

        /// <summary>
        /// IndividualID Person
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Fecha de modificacion
        /// </summary>
        [DataMember]
        public DateTime? ModificationDate { get; set; }

        /// <summary>
        /// Observacion
        /// </summary>
        [DataMember]
        public string Observation { get; set; }

        ///// <summary>
        ///// Id Tipo de baja
        ///// </summary>
        //[DataMember]
        //public int? ProviderDeclinedTypeId { get; set; }

        ///// <summary>
        ///// Fecha de creación
        ///// </summary>
        //[DataMember]
        //public DateTime CreationDate { get; set; }

    }
}
