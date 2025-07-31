using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// Modelo de aplicación de pagos
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador de Módulo
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// Identificador del recurso origen de aplicación
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// Identificador de la sucursal
        /// </summary>
        public int BranchId { get; set; }

        /// <summary>
        /// Fecha de registro
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// Fecha contable
        /// </summary>
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Identificador de la persona a quien se aplica el pago
        /// </summary>
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Identificador de la transacción técnica
        /// </summary>
        public int TechnicalTransaction { get; set; }

        /// <summary>
        /// Valor de verificación
        /// </summary>
        public Amount VerificationValue { get; set; }

        /// <summary>
        /// Indica si es temporal
        /// </summary>
        public bool IsTemporal { get; set; }

        /// <summary>
        /// Elementos de la aplicación
        /// </summary>
        public List<TransactionType> ApplicationItems { get; set; }

        /// <summary>
        /// DailyAccountingTransaction
        /// </summary>
        public DailyAccountingTransaction DailyAccountingTransaction { get; set; }
        /// <summary>
        /// PaymentRequestTransaction
        /// </summary>
        public PaymentRequestTransaction PaymentRequestTransaction { get; set; }
        /// <summary>
        /// ApplicationPremiumTransaction
        /// </summary>
        public ApplicationPremiumTransaction ApplicationPremiumTransaction { get; set; }
        /// <summary>
        /// TempApplicationPremiumComponents
        /// </summary>
        public List<TempApplicationPremiumComponent> TempApplicationPremiumComponents { get; set; }
        /// <summary>
        /// TempApplicationPremium
        /// </summary>
        public List<TempApplicationPremium> TempApplicationPremium { get; set; }
        /// <summary>
        /// ApplicationPremiumTransactions
        /// </summary>
        public ApplicationPremiumTransaction ApplicationPremiumTransactions { get; set; }
        /// <summary>
        /// ApplicationPremiumCommision
        /// </summary>
        public List<ApplicationPremiumCommision> ApplicationPremiumCommision { get; set; }
        /// <summary>
        /// ApplicationPremiumCommision
        /// </summary>
        public List<ApplicationPremium> ApplicationPremiums { get; set; }
        /// <summary>
        /// ApplicationPremiumCommision
        /// </summary>
        public List<ApplicationPremium> TempApplicationPremiums { get; set; }
        /// <summary>
        /// BrokersCheckingAccountTransaction
        /// </summary>
        public BrokersCheckingAccountTransaction BrokersCheckingAccountTransaction { get; set; }
        /// <summary>
        /// CoInsuranceCheckingAccountTransaction
        /// </summary>
        public CoInsuranceCheckingAccountTransaction CoInsuranceCheckingAccountTransaction { get; set; }
        /// <summary>
        /// ReInsuranceCheckingAccountTransaction
        /// </summary>
        public ReInsuranceCheckingAccountTransaction ReInsuranceCheckingAccountTransaction { get; set; }
        /// <summary>
        /// ReInsuranceCheckingAccountTransaction
        /// </summary>
        public ClaimsPaymentRequestTransaction ClaimsPaymentRequestTransaction { get; set; }




    }
}
