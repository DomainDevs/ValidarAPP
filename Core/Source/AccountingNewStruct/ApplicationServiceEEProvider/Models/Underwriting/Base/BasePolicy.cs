using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base
{
    /// <summary>
    /// Poliza Base
    /// </summary>

    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BasePolicy : Extension
    {
        /// <summary>
        /// Identificador del temporal
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Número de Póliza
        /// </summary>
        [DataMember]
        public decimal DocumentNumber { get; set; }

        /// <summary>
        /// Fecha de Emisión
        /// </summary>
        [DataMember]
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Vigencia incial
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Vigencia final
        /// </summary>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Tipo de negocio
        /// </summary>
        [DataMember]
        public string BusinessTypeDescription { get; set; }

        /// <summary>
        /// Id Usuario
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Calcula prima minima
        /// </summary>
        [DataMember]
        public bool? CalculateMinPremium { get; set; }

        /// <summary>
        /// Fecha de inicio de creacion
        /// </summary>
        [DataMember]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Dias efectivos
        /// </summary>
        [DataMember]
        public int EffectPeriod { get; set; }

        /// <summary>
        /// Hora
        /// </summary> 
        [DataMember]
        public string TimeHour { get; set; }

        /// <summary>
        /// Minutos
        /// </summary>        
        [DataMember]
        public string TimeMinutes { get; set; }

        /// <summary>
        /// Esta Persistido
        /// </summary>        
        [DataMember]
        public bool IsPersisted { get; set; }

        [DataMember]
        public int? SubEndorsementType { get; set; }

        /// <summary>
        /// Pérdida Total
        /// </summary>
        [DataMember]
        public bool HasTotalLoss { get; set; }

        /// <summary>
        /// Cantidad de Siniestros
        /// </summary>
        [DataMember]
        public int SinisterQuantity { get; set; }

        /// <summary>
        /// Saldo de Cartera
        /// </summary>
        [DataMember]
        public decimal PortfolioBalance { get; set; }

        /// <summary>
        /// Descripcion Tipo de temporal
        /// </summary>
        [DataMember]
        public string TemporalTypeDescription { get; set; }

        /// <summary>
        /// Tipo de negocio
        /// </summary>
        [DataMember]
        public Enums.BusinessType? BusinessType { get; set; }

    }
}
