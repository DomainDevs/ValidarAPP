using Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables
{
    [DataContract]
    public class PaymentRequest
    {
        /// <summary>
        /// Id 
        /// </summary>        
        public int Id { get; set; }

        /// <summary>
        /// PaymentRequestTypes
        /// </summary>        
        public PaymentRequestTypes PaymentRequestType { get; set; }

        /// <summary>
        /// PaymentRequestNumber
        /// </summary>        
        public PaymentRequestNumber PaymentRequestNumber { get; set; }

        /// <summary>
        /// MovementType 
        /// </summary>
        /// <returns></returns>
        public MovementType MovementType { get; set; }

        /// <summary>
        /// Company
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// Branch
        /// </summary>
        public Branch Branch { get; set; }

        /// <summary>
        /// SalePoint
        /// </summary>
        public SalePoint SalePoint { get; set; }

        /// <summary>
        /// PersonType 
        /// </summary>        
        public PersonType PersonType { get; set; }

        /// <summary>
        /// Beneficiary 
        /// </summary>               
        public Individual Beneficiary { get; set; }

        /// <summary>
        /// PaymentMethod
        /// </summary>
        public EEProvider.Models.Payments.PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Currency 
        /// </summary>                
        public Currency Currency { get; set; }

        /// <summary>
        /// TotalAmount 
        /// </summary>        
        public Amount TotalAmount { get; set; }

        /// <summary>
        /// LocalAmount 
        /// </summary>        
        public Amount LocalAmount { get; set; }

        /// <summary>
        /// ExchangeRate
        /// </summary>
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        /// RegisterDate
        /// </summary>
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// EstimatedDate
        /// </summary>
        public DateTime EstimatedDate { get; set; }

        /// <summary>
        /// AccountingDate
        /// </summary>
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Description 
        /// </summary>        
        public string Description { get; set; }

        /// <summary>
        /// Vouchers
        /// </summary>
        public List<Voucher> Vouchers { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Transaction
        /// </summary>
        public Collect.Transaction Transaction { get; set; }
    }
}
