using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables
{
    [DataContract]
    public class PaymentRequestDTO
    {
        /// <summary>
        /// Id 
        /// </summary>        
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// PaymentRequestTypes
        /// </summary>        
        [DataMember]
        public int PaymentRequestType { get; set; }

        /// <summary>
        /// PaymentRequestNumber
        /// </summary>        
        [DataMember]
        public PaymentRequestNumberDTO PaymentRequestNumber { get; set; }

        /// <summary>
        /// MovementType 
        /// </summary>
        /// <returns></returns>
        [DataMember]
        public MovementTypeDTO MovementType { get; set; }


        /// <summary>
        /// Company
        /// </summary>
        [DataMember]
        public CompanyDTO Company { get; set; }

        /// <summary>
        /// Branch
        /// </summary>
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        /// SalePoint
        /// </summary>
        [DataMember]
        public SalePointDTO SalePoint { get; set; }

        /// <summary>
        /// PersonType 
        /// </summary>        
        [DataMember]
        public PersonTypeDTO PersonType { get; set; }

        /// <summary>
        /// Beneficiary 
        /// </summary>               
        [DataMember]
        public IndividualDTO Beneficiary { get; set; }

        /// <summary>
        /// PaymentMethod
        /// </summary>
        [DataMember]
        public PaymentMethodDTO PaymentMethod { get; set; }

        /// <summary>
        /// Currency 
        /// </summary>                
        [DataMember]
        public CurrencyDTO Currency { get; set; }

        /// <summary>
        /// TotalAmount 
        /// </summary>        
        [DataMember]
        public AmountDTO TotalAmount { get; set; }

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
        /// RegisterDate
        /// </summary>
        [DataMember]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// EstimatedDate
        /// </summary>
        [DataMember]
        public DateTime EstimatedDate { get; set; }

        /// <summary>
        /// AccountingDate
        /// </summary>
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// Description 
        /// </summary>        
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Vouchers
        /// </summary>
        [DataMember]
        public List<VoucherDTO> Vouchers { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Transaction
        /// </summary>
        [DataMember]
        public TransactionDTO Transaction { get; set; }

        [DataMember]
        public int TemporalId { get; set; }

        [DataMember]
        public DateTime? PaymentDate { get; set; }

        [DataMember]
        public ClaimDTO Claim { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public DateTime RegistrationDate { get; set; }

        [DataMember]
        public PaymentRequestTypeDTO Type { get; set; }

    }
}
