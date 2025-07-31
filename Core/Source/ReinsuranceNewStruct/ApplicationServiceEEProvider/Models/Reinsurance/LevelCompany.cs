using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Modelo de Niveles de Contrato y sus Compañias
    /// </summary>
    [DataContract]
    public class LevelCompany
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
        public Agent Agent { get; set; }

        /// <summary>
        /// Compañia
        /// </summary>
        [DataMember]
        public Company Company { get; set; }

        /// <summary>
        /// Porcentaje Cedido (aplica a suma asegurada, prima, recobro de salvamiento y suma siniestrada)
        /// </summary>
        [DataMember]
        public decimal GivenPercentage{ get; set; }

        /// <summary>
        /// Suma Cedida del Importe Asegurado o Suma Siniestrada (calculada)
        /// </summary>
        public Amount GivenAmount { get; set; }

        /// <summary>
        /// Prima Cedida (calculada)
        /// </summary>
        public Amount GivenPremiumAmount { get; set; }

        /// <summary>
        /// Suma Cedida de Recobro de Salvamiento
        /// </summary>
        public Amount GivenRecoverySalvageAmount { get; set; }

        /// <summary>
        /// Porcentaje de comisión
        /// </summary>
        [DataMember]
        public decimal? ComissionPercentage { get; set; }

        /// <summary>
        /// Nivel del contrato
        /// </summary>
         [DataMember]
        public Level ContractLevel { get; set; }
        
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
        public List<Installment> PaymentSchedule { get; set; }

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