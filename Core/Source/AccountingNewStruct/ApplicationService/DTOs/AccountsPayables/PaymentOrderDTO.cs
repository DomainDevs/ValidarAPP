
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables
{
    /// <summary>
    /// PaymentOrder: 
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class PaymentOrderDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// PersonType: Tipo de Beneficiario
        /// </summary> 
        [DataMember]
        public PersonTypeDTO PersonType { get; set; }

        /// <summary>
        /// Beneficiary: Beneficiario
        /// </summary> 
        [DataMember]
        public IndividualDTO Beneficiary { get; set; }

        /// <summary>
        /// Branch: Sucursal
        /// </summary> 
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        /// Amount: Importe
        /// </summary> 
        [DataMember]
        public AmountDTO Amount { get; set; }

        /// <summary>
        /// LocalAmount 
        /// </summary>        
        [DataMember]
        public AmountDTO LocalAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRateDTO ExchangeRate { get; set; }

        /// <summary>
        /// Company: Compañia
        /// </summary> 
        [DataMember]
        public CompanyDTO Company { get; set;}

        /// <summary>
        /// BranchPay: Sucursal del Pago
        /// </summary> 
        [DataMember]
        public BranchDTO BranchPay { get; set; }
        /// <summary>
        /// PaymentSource: Origen de Pago
        /// </summary> 
        [DataMember]
        public ConceptSourceDTO PaymentSource { get; set; }
        /// <summary>
        /// PaymentDate: Fecha de Pago
        /// </summary> 
        [DataMember]
        public DateTime PaymentDate { get; set; }
        /// <summary>
        /// EstimatedPaymentDate: Fecha Estimada de Pago
        /// </summary> 
        [DataMember]
        public DateTime EstimatedPaymentDate { get; set; }
        /// <summary>
        /// PaymentMethod: Metodo de Pago
        /// </summary> 
        [DataMember]
        public PaymentMethodDTO PaymentMethod { get; set; }

        /// <summary>
        /// PayTo: Pagar a 
        /// </summary> 
        [DataMember]
        public string PayTo { get; set; }

        /// <summary>
        /// BankAccountPerson: Cuenta Bancaria Persona
        /// </summary>        
        [DataMember]
        public BankAccountPersonDTO BankAccountPerson { get; set; }
        
        /// <summary>
        /// Status: Estado 
        /// </summary>        
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// Imputation: Imputacion 
        /// </summary> 
        [DataMember]
        public ApplicationDTO Imputation { get; set; }

        /// <summary>
        /// Transaction: Transaccion 
        /// </summary> 
        [DataMember]
        public TransactionDTO Transaction { get; set; }

      
        /// <summary>
        /// IsTemporal: Se trata de un transaccion temporal?
        /// </summary>        
        [DataMember]
        public bool IsTemporal { get; set; }

        /// <summary>        
        /// AccountingDate: Fecha contable
        /// </summary>        
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>        
        /// CancellationDate: Fecha de anulación de la Orden de Pago
        /// </summary>        
        [DataMember]
        public DateTime? CancellationDate { get; set; }

        /// <summary>        
        /// User: Usuario que modifica la orden de pago
        /// </summary>        
        [DataMember]
        public int UserId { get; set; }


        /// <summary>
        /// Observaciones de la orden de pago
        /// </summary>
        [DataMember]
        public string Observation { get; set; }
    }
}
