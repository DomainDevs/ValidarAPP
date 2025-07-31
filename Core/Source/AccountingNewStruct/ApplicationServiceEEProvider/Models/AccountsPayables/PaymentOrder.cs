using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables
{
    /// <summary>
    /// PaymentOrder: 
    /// </summary>
    /// <returns></returns>
    [DataContract]
    public class PaymentOrder
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
        public PersonType PersonType { get; set; }

        /// <summary>
        /// Beneficiary: Beneficiario
        /// </summary> 
        [DataMember]
        public Individual Beneficiary { get; set; }

        /// <summary>
        /// Branch: Sucursal
        /// </summary> 
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// Amount: Importe
        /// </summary> 
        [DataMember]
        public Amount Amount { get; set; }

        /// <summary>
        /// LocalAmount 
        /// </summary>        
        [DataMember]
        public Amount LocalAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        [DataMember]
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        /// Company: Compañia
        /// </summary> 
        [DataMember]
        public Company Company { get; set;}

        /// <summary>
        /// BranchPay: Sucursal del Pago
        /// </summary> 
        [DataMember]
        public Branch BranchPay { get; set; }

        /// <summary>
        /// PaymentSource: Origen de Pago
        /// </summary> 
        [DataMember]
        public ConceptSource PaymentSource { get; set; }

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
        public Payments.PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// PayTo: Pagar a 
        /// </summary> 
        [DataMember]
        public string PayTo { get; set; }

        /// <summary>
        /// BankAccountPerson: Cuenta Bancaria Persona
        /// </summary>        
        [DataMember]
        public BankAccountPerson BankAccountPerson { get; set; }
        
        /// <summary>
        /// Status: Estado 
        /// </summary>        
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        /// Imputation: Imputacion 
        /// </summary> 
        [DataMember]
        public Imputations.Application Imputation { get; set; }

        /// <summary>
        /// Transaction: Transaccion 
        /// </summary> 
        [DataMember]
        public Transaction Transaction { get; set; }

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
