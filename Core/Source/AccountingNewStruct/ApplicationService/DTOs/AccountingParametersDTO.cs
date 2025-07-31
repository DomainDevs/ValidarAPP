using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class AccountingParametersDTO
    {
        /// <summary>
        /// TypeId
        /// </summary>
        [DataMember]
        public int TypeId { get; set; }

        /// <summary>
        /// BillId
        /// </summary>
        [DataMember]
        public int BillId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// TechnicalTransaction
        /// </summary>
        [DataMember]
        public int TechnicalTransaction {get; set;}
        /// <summary>
        /// CollectTechnicalTransaction
        /// </summary>
        [DataMember]
        public int CollectTechnicalTransaction { get; set; }

        /// <summary>
        /// PaymentCode
        /// </summary>
        [DataMember]
        public int PaymentCode { get; set; }

        /// <summary>
        /// CollectPaymentCode
        /// </summary>
        [DataMember]
        public int CollectPaymentCode { get; set; }

        /// <summary>
        /// AccountingDate
        /// </summary>
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        /// AccountingListParameters
        /// </summary>
        [DataMember]
        public List<AccountingListParametersDTO> JournalEntryListParameters { get; set; }

        /// <summary>
        /// Application
        /// </summary>
        [DataMember]
        public ApplicationLedgerDTO Application { get; set; }

        /// <summary>
        /// ApplicationItem
        /// </summary>
        [DataMember]
        public ApplicationItemDTO ApplicationItem { get; set; }

        /// <summary>
        /// Application items
        /// </summary>
        [DataMember]
        public List<ApplicationJournalEntryDTO> ApplicationItems { get; set; }

        /// <summary>
        /// Branch Id
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        /// Sale point Id
        /// </summary>
        [DataMember]
        public int SalePointId { get; set; }

        /// <summary>
        /// Bridge accounting id
        /// </summary>
        [DataMember]
        public int BridgeAccoutingId { get; set; }

        /// <summary>
        /// Bridge accounting id
        /// </summary>
        [DataMember]
        public string BridgePackageCode { get; set; }

        /// <summary>
        /// Original General Ledger
        /// </summary>
        [DataMember]
        public OriginalGeneralLedgerDTO OriginalGeneralLedger { get; set; }
    }
}