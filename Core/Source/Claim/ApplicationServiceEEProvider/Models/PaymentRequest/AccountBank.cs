
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    [DataContract]
    public class AccountBank
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int? AccountTypeId { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public int? BankId { get; set; }

        [DataMember]
        public bool? Enabled { get; set; }

        [DataMember]
        public bool? Default { get; set; }

        [DataMember]
        public int? CurrencyId { get; set; }

        [DataMember]
        public int? GeneralLedgerId { get; set; }

        [DataMember]
        public DateTime? DisabledDate { get; set; }

        [DataMember]
        public int? BranchId { get; set; }
    }
}
