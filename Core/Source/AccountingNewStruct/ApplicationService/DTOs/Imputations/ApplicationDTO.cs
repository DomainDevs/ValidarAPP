using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// Modelo de aplicación de pagos
    /// </summary>
    [DataContract]
    public class ApplicationDTO
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
        /// Identificador de la sucursal
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

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
        /// Identificador de la transacción técnica
        /// </summary>
        [DataMember]
        public int TechnicalTransaction { get; set; }

        /// <summary>
        /// Listado de primas asociadas
        /// </summary>
        [DataMember]
        public List<ApplicationPremiumDTO> Premiums { get; set; }

        /// <summary>
        /// Valor de verificación
        /// </summary>
        [DataMember]
        public AmountDTO VerificationValue { get; set; }

        /// <summary>
        /// ApplicationItems: elementos de la aplicación
        /// </summary>        
        [DataMember]
        public List<TransactionTypeDTO> ApplicationItems { get; set; }

        /// <summary>
        /// Indica si es temporal
        /// </summary>
        [DataMember]
        public bool IsTemporal { get; set; }
    }
}
