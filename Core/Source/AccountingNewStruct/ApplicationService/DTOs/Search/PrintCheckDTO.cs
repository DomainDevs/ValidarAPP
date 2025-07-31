using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PrintCheckDTO
    {
        [DataMember]
        public string AddressCompany { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string BeneficiaryName { get; set; }

        [DataMember]
        public int BranchCode { get; set; }

        [DataMember]
        public int CheckNumber { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string EstimatedPaymentDate { get; set; }

        [DataMember]
        public int NumberPaymentOrder { get; set; }

        [DataMember]
        public int PaymentSourceCode { get; set; }

        [DataMember]
        public int AccountBankId { get; set; }

        [DataMember]
        public int CheckPaymentOrderCode { get; set; }

        [DataMember]
        public string CourierName { get; set; }

        [DataMember]
        public string DeliveryDate { get; set; }

        [DataMember]
        public string RefundDate { get; set; }

        [DataMember]
        public string DescriptionCity { get; set; }
    }
}
