using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// Modelo de aplicación de pagos temporal
    /// </summary>
    [DataContract]
    public class TempApplicationDTO
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador de Módulo
        /// </summary>
        [DataMember]
        public int ModuleId { get; set; }

        /// <summary>
        /// Identificador del recurso origen de aplicación
        /// </summary>
        [DataMember]
        public int SourceId { get; set; }

        /// <summary>
        /// Fecha de registro
        /// </summary>
        [DataMember]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// Fecha contable
        /// </summary>
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Identificador de la persona a quien se aplica el pago
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador del usuario
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Primas asociadas a la aplicación
        /// </summary>
        [DataMember]
        public List<TempApplicationPremiumDTO> Premiums { get; set; }
    }
}