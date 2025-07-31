using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{

    [DataContract]
    public class DepositCheckInformationDTO 
    {
        [DataMember]
        public int PaymentCode { get; set; }
        [DataMember]
        public string RegisterDate { get; set; }
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public string ReceivingAccountNumber { get; set; }
        [DataMember]
        public int BankCode { get; set; }
        [DataMember]
        public string BankDescription { get; set; }
        [DataMember]
        public string DepositRegisterDate { get; set; }
        [DataMember]
        public string PaymentBallotNumber { get; set; }
        [DataMember]
        public int PaymentBallotCode { get; set; }
    }


}
