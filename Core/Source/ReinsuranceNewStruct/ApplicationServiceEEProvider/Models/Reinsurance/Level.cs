#region Using
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;



#endregion

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo para Niveles de contrato
    /// </summary>
    [DataContract]
    public class Level
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

        ///// <summary>
        ///// Suma del Importe Asegurado Retenida del Nivel
        ///// </summary>
        //public Amount RetainedInsuredAmount { get; set; }

        ///// <summary>
        ///// Suma de Recobro de Salvamento Retenida del Nivel
        ///// </summary>
        //public Amount RetainedRecoverySalvageAmount { get; set; }

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
        public List<LevelCompany> ContractLevelCompanies { get; set; }

        /// <summary>
        /// Contrato
        /// </summary>
        [DataMember]
        public Contract Contract { get; set; }

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
         public List<LevelPayment> LevelPayments { get; set; }

        /// <summary>
        /// LevelRestores
        /// </summary>
        [DataMember]
        public List<LevelRestore> LevelRestores { get; set; }




    }
}