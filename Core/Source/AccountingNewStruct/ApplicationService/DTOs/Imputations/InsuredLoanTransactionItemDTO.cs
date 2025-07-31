using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Imputations
{
    /// <summary>
    /// InsuredLoanTransactionItem:  Item Transaccion de Préstamos de Asegurados
    /// </summary>
    [DataContract]
    public class InsuredLoanTransactionItemDTO
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
        public ApplicationDTO Imputation { get; set; }

        /// <summary>
        /// LoanNumber: Número préstamo
        /// </summary>        
        [DataMember]
        public int LoanNumber { get; set; }

        /// <summary>
        /// Insured:  Asegurado
        /// </summary>        
       [DataMember]
        public IndividualDTO Insured { get; set; }

        /// <summary>
        /// AccountingNature: Naturaleza de la transacción
        /// </summary>
        [DataMember]
        public int AccountingNature { get; set; }

        /// <summary>
        /// Capital: Importe capital
        /// </summary>
        [DataMember]
        public AmountDTO Capital { get; set; }

        /// <summary>
        /// ExchangeRate: Tasa de cambio
        /// </summary>
        [DataMember]
        public ExchangeRateDTO ExchangeRate { get; set; }

        /// <summary>
        /// CurrentInterest: Interés  actual
        /// </summary>
        [DataMember]
        public AmountDTO CurrentInterest { get; set; }

        /// <summary>
        /// ExchangeRateCurrent: Tasa de cambio
        /// </summary>
        [DataMember]
        public ExchangeRateDTO ExchangeRateCurrent { get; set; }

        /// <summary>
        /// PreviousInterest: Interés previo 
        /// </summary>
        [DataMember]
        public AmountDTO PreviousInterest { get; set; }

        /// <summary>
        /// ExchangeRatePrevious: Tasa de cambio
        /// </summary>
        [DataMember]
        public ExchangeRateDTO ExchangeRatePrevious { get; set; }

        /// <summary>
        /// LocalAmountPrevious: Interés previo en moneda local
        /// </summary>
        [DataMember]
        public AmountDTO LocalAmountPrevious { get; set; }

    }

}
