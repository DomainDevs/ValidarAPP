using Sistran.Core.Application.AccountingServices.Enums;
//Sistran
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations
{
    /// <summary>
    /// InsuredLoanTransactionItem:  Item Transaccion de Préstamos de Asegurados
    /// </summary>
    [DataContract]
    public class InsuredLoanTransactionItem
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Imputation: Imputación
        /// </summary>
        [DataMember]
        public Application Imputation { get; set; }

        /// <summary>
        /// LoanNumber: Número préstamo
        /// </summary>        
        [DataMember]
        public int LoanNumber { get; set; }

        /// <summary>
        /// Insured:  Asegurado
        /// </summary>        
       [DataMember]
        public Individual Insured { get; set; }

        /// <summary>
        /// AccountingNature: Naturaleza de la transacción
        /// </summary>
        [DataMember]
        public AccountingNature AccountingNature { get; set; }

        /// <summary>
        /// Capital: Importe capital
        /// </summary>
        [DataMember]
        public Amount Capital { get; set; }

        /// <summary>
        /// ExchangeRate: Tasa de cambio
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        /// CurrentInterest: Interés  actual
        /// </summary>
        [DataMember]
        public Amount CurrentInterest { get; set; }

        /// <summary>
        /// ExchangeRateCurrent: Tasa de cambio
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRateCurrent { get; set; }

        /// <summary>
        /// PreviousInterest: Interés previo 
        /// </summary>
        [DataMember]
        public Amount PreviousInterest { get; set; }

        /// <summary>
        /// ExchangeRatePrevious: Tasa de cambio
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRatePrevious { get; set; }

        /// <summary>
        /// LocalAmountPrevious: Interés previo en moneda local
        /// </summary>
        [DataMember]
        public Amount LocalAmountPrevious { get; set; }
    }
}
