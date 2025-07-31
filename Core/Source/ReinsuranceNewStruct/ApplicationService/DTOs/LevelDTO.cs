using Sistran.Core.Application.ReinsuranceServices.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Modelo para Niveles de contrato
    /// </summary>
    [DataContract]
    public class LevelDTO
    {
        /// <summary>
        /// Identificador �nico del modelo
        /// </summary>
        [DataMember]
        public int ContractLevelId { get; set; }

        /// <summary>
        /// N�mero de nivel
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// Especifica el l�mite de contrato
        /// </summary>
        [DataMember]
        public decimal? ContractLimit { get; set; }

        /// <summary>
        /// Especifica el l�mite porcentaje asignado
        /// </summary>
        [DataMember]
        public decimal AssignmentPercentage { get; set; }

        /// <summary>
        /// L�mite de retenci�n
        /// </summary>
        [DataMember]
        public decimal? RetentionLimit { get; set; }

        /// <summary>
        /// N�mero de l�neas
        /// </summary>
        [DataMember]
        public decimal? LinesNumber { get; set; }

        /// <summary>
        /// L�mite de eventos
        /// </summary>
        [DataMember]
        public decimal? EventLimit { get; set; }

        /// <summary>
        /// Listado de Niveles de Contrato y sus Compa�ias
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