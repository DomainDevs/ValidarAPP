using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PaymentBallotDTO
    {
        [DataMember]
        public int DepositBallotBankId { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string DepositBallotBankDescription { get; set; }
        [DataMember]
        public string DepositBallotAccountNumber { get; set; }
        [DataMember]
        public int DepositBallotId { get; set; }
        [DataMember]
        public Decimal DepositBallotAmount { get; set; }
        [DataMember]
        public string DepositBallotNumber { get; set; }
        [DataMember]
        public Decimal DepositBallotCashAmount { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public DateTime DepositBallotRegisterDate { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public int Rows { get; set; }
        //BE
        [DataMember]
        public int PaymentId { get; set; }  //id_mcb  
        [DataMember]
        public string LedgerAccount { get; set; }  //cod_cta_cb 

    }
}
