using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class AccountingPaymentRequestDTO
    {
        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public List<VoucherDTO> Vocuher { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public DateTime AccountingDate { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }
    }
}
