using Sistran.Core.Application.ReinsuranceServices.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Modelo de Niveles de Contrato y sus Compañias
    /// </summary>
    [DataContract]
    public class LevelCompanyDTO
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        [DataMember]
        public int LevelCompanyId { get; set; }

        /// <summary>
        /// Broker
        /// </summary>
        [DataMember]
        public AgentDTO Agent { get; set; }

        /// <summary>
        /// Compañia
        /// </summary>
        [DataMember]
        public CompanyDTO Company { get; set; }

        /// <summary>
        /// Porcentaje Cedido (aplica a suma asegurada, prima, recobro de salvamiento y suma siniestrada)
        /// </summary>
        [DataMember]
        public decimal GivenPercentage{ get; set; }

        /// <summary>
        /// Suma Cedida del Importe Asegurado o Suma Siniestrada (calculada)
        /// </summary>
        public AmountDTO GivenAmount { get; set; }

        /// <summary>
        /// Prima Cedida (calculada)
        /// </summary>
        public AmountDTO GivenPremiumAmount { get; set; }

        /// <summary>
        /// Suma Cedida de Recobro de Salvamiento
        /// </summary>
        public AmountDTO GivenRecoverySalvageAmount { get; set; }

        /// <summary>
        /// Porcentaje de comisión
        /// </summary>
        [DataMember]
        public decimal? ComissionPercentage { get; set; }

        /// <summary>
        /// Nivel del contrato
        /// </summary>
         [DataMember]
        public LevelDTO ContractLevel { get; set; }
        
        /// <summary>
        /// Porcentaje de reserva de prima
        /// </summary>
        [DataMember]
        public decimal?  ReservePremiumPercentage { get; set; }

        /// <summary>
        /// Porcentaje de Interés sobre la Liberación de Reservas
        /// </summary>
        [DataMember]
        public decimal? InterestReserveRelease { get; set; }

        /// <summary>
        /// Porcentaje participacion Utilidad
        /// </summary>
        [DataMember]
        public decimal UtilitySharePercentage { get; set; }

        /// <summary>
        /// Porcentaje Comision Utilidad
        /// </summary>
        [DataMember]
        public decimal AdditionalCommissionPercentage { get; set; }

        /// <summary>
        /// Tipo de Presentacion Informacion
        /// </summary>
        [DataMember]
        public PresentationInformationTypes PresentationInformationType { get; set; }

        /// <summary>
        /// Porcentaje Gasto Reasegurador
        /// </summary>
        [DataMember]
        public decimal ReinsuranceExpensePercentage { get; set; }

        /// <summary>
        /// Porcentaje Arrstre de Perdida
        /// </summary>
        [DataMember]
        public decimal DragLossPercentage { get; set; }

        /// <summary>
        /// Comision Intermediario
        /// </summary>
        [DataMember]
        public bool IntermediaryCommission { get; set; }

        /// <summary>
        /// Porcentaje Comision Siniestro
        /// </summary>
        [DataMember]
        public decimal ClaimCommissionPercentage { get; set; }

        /// <summary>
        /// Porcentaje Comision Diferencial
        /// </summary>
        [DataMember]
        public decimal DifferentialCommissionPercentage { get; set; }

        /// <summary>
        /// PaymentSchedule
        /// </summary>
        [DataMember]
        public List<InstallmentDTO> PaymentSchedule { get; set; }

        /// <summary>
        /// Commission
        /// </summary>
        [DataMember]
        public decimal Commission { get; set; }

        /// <summary>
        /// DepositPercentage
        /// </summary>
        [DataMember]
        public decimal DepositPercentage { get; set; }

        /// <summary>
        /// Reserve
        /// </summary>
        [DataMember]
        public decimal Reserve { get; set; }

        /// <summary>
        /// InterestOnReserve
        /// </summary>
        [DataMember]
        public decimal InterestOnReserve { get; set; }

        /// <summary>
        /// InterestOnReservePercentage
        /// </summary>
        [DataMember]
        public decimal InterestOnReservePercentage { get; set; }

        /// <summary>
        /// PremiumToPay
        /// </summary>
        [DataMember]
        public decimal PremiumToPay { get; set; }

        /// <summary>
        /// DepositReleaseDate
        /// </summary>
        [DataMember]
        public DateTime DepositReleaseDate { get; set; }
    }
}