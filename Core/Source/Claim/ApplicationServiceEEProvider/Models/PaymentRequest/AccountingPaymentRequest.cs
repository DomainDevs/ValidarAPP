using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    [DataContract]
    public class AccountingPaymentRequest
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public int BranchId { get; set; }

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

        [DataMember]
        public int TechnicalTransaction { get; set; }

        [DataMember]
        public int AccountingAccountId { get; set; }

        [DataMember]
        public string AccountingAccountNumber { get; set; }

        [DataMember]
        public int AccountingNatureId { get; set; }

        [DataMember]
        public int SalePointId { get; set; }

        [DataMember]
        public int BusinessTypeId { get; set; }

        [DataMember]
        public int PaymentSourceId { get; set; }

        [DataMember]
        public int SalvageId { get; set; }

        [DataMember]
        public int RecoveryId { get; set; }

        [DataMember]
        public List<AccountingVoucher> Vouchers { get; set; }

        [DataMember]
        public List<AccountingCoInsuranceAssigned> CoInsuranceAssigneds { get; set; }

    }
}
