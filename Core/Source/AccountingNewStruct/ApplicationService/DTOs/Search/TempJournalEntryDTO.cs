using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class TempJournalEntryDTO
    {
        [DataMember]
        public int TempJournalEntryId { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public int SalesPointId { get; set; }
        [DataMember]
        public string SalesPointName { get; set; }
        [DataMember]
        public int CompanyId { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public int PersonTypeId { get; set; }
        [DataMember]
        public string PersonTypeName { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string PayerName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int StatusId { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string AccountingDate { get; set; }

    }
}
