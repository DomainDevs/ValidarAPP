using Sistran.Core.Integration.ReinsuranceIntegrationServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance
{
    /// <summary>
    /// Modelo para Niveles de contrato
    /// </summary>
    [DataContract]
    public class LevelDTO
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        [DataMember]
        public int ContractLevelId { get; set; }

        /// <summary>
        /// Número de nivel
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Especifica el límite de contrato
        /// </summary>
        [DataMember]
        public decimal? ContractLimit { get; set; }

        /// <summary>
        /// Especifica el límite porcentaje asignado
        /// </summary>
        [DataMember]
        public decimal AssignmentPercentage { get; set; }

        /// <summary>
        /// Límite de retención
        /// </summary>
        [DataMember]
        public decimal? RetentionLimit { get; set; }

        /// <summary>
        /// Número de líneas
        /// </summary>
        [DataMember]
        public int? LinesNumber { get; set; }

        /// <summary>
        /// Límite de eventos
        /// </summary>
        [DataMember]
        public decimal? EventLimit { get; set; }

        /// <summary>
        /// Listado de Niveles de Contrato y sus Compañias
        /// </summary>
        [DataMember]
        public List<LevelCompanyDTO> ContractLevelCompanies { get; set; }

        /// <summary>
        /// Contrato
        /// </summary>
        [DataMember]
        public ContractDTO Contract { get; set; }

        /// <summary>
        /// Porcentaje Tasa Fija
        /// </summary>
        [DataMember]
        public decimal FixedRatePercentage { get; set; }

        /// <summary>
        /// Porcentaje Tasa Minima
        /// </summary>
        [DataMember]
        public decimal MinimumRatePercentage { get; set; }

        /// <summary>
        /// Porcentaje Tasa Maxima
        /// </summary>
        [DataMember]
        public decimal MaximumRatePercentage { get; set; }

        /// <summary>
        /// Porcentaje Reajuste
        /// </summary>
        [DataMember]
        public decimal AdjustmentPercentage { get; set; }

        /// <summary>
        /// Tasa Vida
        /// </summary>
        [DataMember]
        public decimal LifeRate { get; set; }

        /// <summary>
        /// Tipo de Calculo
        /// </summary>
        [DataMember]
        public CalculationTypes CalculationType { get; set; }

        /// <summary>
        /// Aplicar Sobre
        /// </summary>
        [DataMember]
        public ApplyOnTypes ApplyOnType { get; set; }

        /// <summary>
        /// Limite Agregado Annual
        /// </summary>
        [DataMember]
        public decimal AnnualAddedLimit { get; set; }

        /// <summary>
        /// Tipo Prima
        /// </summary>
        [DataMember]
        public PremiumTypes PremiumType { get; set; }

        /// <summary>
        /// LevelPayments
        /// </summary>
        [DataMember]
        public List<LevelPaymentDTO> LevelPayments { get; set; }

        /// <summary>
        /// LevelRestores
        /// </summary>
        [DataMember]
        public List<LevelRestoreDTO> LevelRestores { get; set; }
    }
}
