using System;
using System.Runtime.Serialization;
using Sistran.Core.Application.Extensions;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseAgent : Extension
    {

        /// <summary>
        /// Individual id
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Fecha de Creación
        /// </summary>
        [DataMember]
        public DateTime DateCurrent { get; set; }

        /// <summary>
        /// Fecha de Baja
        /// </summary>
        [DataMember]
        public DateTime? DateDeclined { get; set; }

        /// <summary>
        /// Fecha de Modificación
        /// </summary>
        [DataMember]
        public DateTime? DateModification { get; set; }

        

        /// <summary>
        /// Anotaciones
        /// </summary>
        [DataMember]
        public string Annotations { get; set; }

        /// <summary>
        /// Anotaciones
        /// </summary>
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Anotaciones
        /// </summary>
        [DataMember]
        public string Locker { get; set; }

        /// <summary>
        /// Anotaciones
        /// </summary>
        [DataMember]
        public bool CommissionDiscountAgreement { get; set; }
    }
}
