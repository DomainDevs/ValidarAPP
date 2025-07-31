using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class LegalPaymentDTO 
    {
        [DataMember]
        public int LegalPaymentId { get; set; }
        [DataMember]
        public int RejectedPaymentId { get; set; }
        [DataMember]
        public DateTime LegalDate { get; set; }
        [DataMember]
        public int PaymentId { get; set; }
        [DataMember]
        public int CollectId { get; set; }
        [DataMember]
        public DateTime DatePayment { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string IssuerName { get; set; }
        [DataMember]
        public int IssuingBankId { get; set; }
        [DataMember]
        public string IssuingAccountNumber { get; set; }
        [DataMember]
        public int UserId { get; set; }

        //PARA INFORMACION DE CONSULTA DE CHEQUES
        [DataMember]
        public int PaymentCode { get; set; }
        [DataMember]
        public int LegalPaymentCode { get; set; }
        [DataMember]
        public string Date { get; set; }
    }
}
