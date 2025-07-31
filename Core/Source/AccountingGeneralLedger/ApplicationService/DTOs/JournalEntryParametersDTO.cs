using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class JournalEntryParametersDTO
    {
        /// <summary>
        /// TypeId
        /// </summary>
        [DataMember]
        public int TypeId { get; set; }

        /// <summary>
        /// BillId
        /// </summary>
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
        public int TechnicalTransaction { get; set; }
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
        /// JournalEntryListParameters
        /// </summary>
        [DataMember]
        public List<JournalEntryListParametersDTO> JournalEntryListParameters { get; set; }

        /// <summary>
        /// JournalEntryListParameters
        /// </summary>
        [DataMember]
        public ApplicationDTO Application { get; set; }

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
        /// Code rule package
        /// </summary>
        [DataMember]
        public string CodeRulePackage { get; set; }

        /// <summary>
        /// Original general ledger
        /// </summary>
        [DataMember]
        public OriginalGeneralLedgerDTO OriginalGeneralLedger { get; set; }
    }
}
