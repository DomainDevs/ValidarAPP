using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.TaxServices;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting.Base
{
    [DataContract]
    public class BasePayerComponent : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tasa
        /// </summary>
        [DataMember]
        public decimal Rate { get; set; }
        /// <summary>
        /// Base de calculo
        /// </summary>
        [DataMember]
        public decimal BaseAmount { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Id impuesto
        /// </summary>
        [DataMember]
        public int? TaxId { get; set; }

        /// <summary>
        /// Id condicion de inpuesto
        /// </summary>
        [DataMember]
        public int? TaxConditionId { get; set; }
        /// <summary>
        /// Tipo de tasa
        /// </summary>
        [DataMember]
        public RateType? RateType { get; set; }

        /// <summary>
        /// Id Cobertura
        /// </summary>
        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        /// <summary>
        /// linea de cobertura
        /// </summary>
        public int LineBusinessId { get; set; }

        [DataMember]
        /// <summary>
        /// sub linea de cobertura
        /// </summary>
        public int SubLineBusinessId { get; set; }
    }
}
