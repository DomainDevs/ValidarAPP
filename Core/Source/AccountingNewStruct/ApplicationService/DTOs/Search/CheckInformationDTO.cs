using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class CheckInformationDTO 
    {
        [DataMember]
        public string DatePayment { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string Holder { get; set; }
        [DataMember]
        public int ReceivingBankCode { get; set; }
        [DataMember]
        public string ReceivingBankName { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        [DataMember]
        public int IssuingBankCode { get; set; }
        [DataMember]
        public string IssuingAccountNumber { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int CollectCode { get; set; }
        [DataMember]
        public int PaymentCode { get; set; }
        [DataMember]
        public string RegisterDate { get; set; }
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public string ReceivingAccountNumber { get; set; }
        [DataMember]
        public int PaymentTicketItemCode { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public int PaymentStatus { get; set; }
        [DataMember]
        public int PaymentTicketStatus { get; set; }
        [DataMember]
        public string PaymentBallotNumber { get; set; }
        [DataMember]
        public int PaymentBallotCode { get; set; }

        [DataMember]
        public int Rows { get; set; }

        //PARA INFORMACION DE CONSULTA DE CHEQUES
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public int CollectConceptCode { get; set; }
        [DataMember]
        public string CollectConceptDescription { get; set; }
        [DataMember]
        public string StatusDescription { get; set; }
        [DataMember]
        public string BankDescription { get; set; } 
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public int TechnicalTransaction { get; set; }

    }

}
